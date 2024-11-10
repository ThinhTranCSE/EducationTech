using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EducationTech.Business.Master;

public class LessonService : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _sessionService;
    public LessonService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISessionService sessionService
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionService = sessionService;
    }

    public async Task<LessonDto> CreateLesson(Lesson_CreateRequestDto requestDto)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create lesson");
        }


        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var courseSectionQuery = _unitOfWork.CourseSections.GetAll();
            courseSectionQuery = courseSectionQuery
                .Include(x => x.Course)
                    .ThenInclude(x => x.Owner)
                .Where(x => x.Id == requestDto.CourseSectionId);

            var courseSection = await courseSectionQuery.FirstOrDefaultAsync();

            if (courseSection == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Course section not found");
            }
            if (courseSection.Course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to create lesson for this course section");
            }

            var createdLesson = new Lesson
            {
                CourseSectionId = requestDto.CourseSectionId,
                Type = requestDto.Type,
                Title = requestDto.Title,
                Order = requestDto.Order
            };

            _unitOfWork.Lessons.Add(createdLesson);
            _unitOfWork.SaveChanges();

            if (requestDto.Type == LessonType.Quiz)
            {
                if (requestDto.Quiz == null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Quiz is required for quiz lesson");
                }
                var quiz = new Quiz
                {
                    LessonId = createdLesson.Id,
                    TimeLimit = requestDto.Quiz.TimeLimit
                };

                _unitOfWork.Quizzes.Add(quiz);
                _unitOfWork.SaveChanges();

                var questions = requestDto.Quiz.Questions.Select(q => new Question
                {
                    Content = q.Content,
                    QuizId = quiz.Id,
                    Answers = q.Answers.Select(a => new Answer
                    {
                        Content = a.Content,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList();

                _unitOfWork.Questions.AddRange(questions);
                _unitOfWork.SaveChanges();
            }
            else if (requestDto.Type == LessonType.Video)
            {
                if (requestDto.VideoId == null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Video id is required for video lesson");
                }
                var videoQuery = _unitOfWork.Videos.GetAll();
                videoQuery = videoQuery
                    .Include(v => v.File.User)
                    .Where(x => x.Id == requestDto.VideoId);
                var video = await videoQuery.FirstOrDefaultAsync();

                if (video == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Video not found");
                }
                if (video.File.UserId != currentUser.Id)
                {
                    throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to use this video");
                }
                video.LessonId = createdLesson.Id;
                _unitOfWork.SaveChanges();
            }
            else
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Invalid lesson type");
            }

            transaction.Commit();
            return _mapper.Map<LessonDto>(createdLesson);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }


    public async Task<LessonDto> GetLessonById(int id)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get lesson detail");
        }

        var query = _unitOfWork.Lessons.GetAll();
        query = query
            .Where(x => x.Id == id)
            .Include(x => x.Video)
            .Include(x => x.Quiz)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Answers)
            .Include(x => x.CourseSection)
                .ThenInclude(x => x.Course);
        var lesson = await query.FirstOrDefaultAsync();
        if (lesson == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Lesson not found");
        }


        var lessonDto = _mapper.Map<LessonDto>(lesson);
        if (lessonDto.Type == LessonType.Quiz && lesson.CourseSection.Course.OwnerId != currentUser.Id)
        {
            foreach (var question in lessonDto.Quiz.Questions)
            {
                foreach (var answer in question.Answers)
                {
                    answer.IsCorrect = null;
                }
            }
        }
        return lessonDto;
    }

    public async Task<Lesson_ValidateQuizResponseDto> SubmitAnswers(Lesson_ValidateQuizRequestDto requestDto)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to submit quizzes");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var quizQuery = _unitOfWork.Quizzes.GetAll();
            quizQuery = quizQuery
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(q => q.Lesson)
                    .ThenInclude(q => q.CourseSection)
                            .ThenInclude(q => q.Course)
                .Where(q => q.Id == requestDto.QuizId);

            var quiz = await quizQuery.FirstOrDefaultAsync();
            if (quiz == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Quiz not found or you do not have permission to submit this quiz");
            }
            var answerBaseIdsSet = new HashSet<int>(quiz.Questions.SelectMany(question => question.Answers).Select(a => a.Id));
            if (requestDto.AnswerIds.Any(a => !answerBaseIdsSet.Contains(a)))
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Invalid answer id");
            }

            foreach (var answerId in requestDto.AnswerIds)
            {
                _unitOfWork.AnswerUsers.Add(new AnswerUser
                {
                    UserId = currentUser.Id,
                    AnswerId = answerId
                });
            }

            _unitOfWork.SaveChanges();
            transaction.Commit();

            var correctAnswerIds = quiz.Questions.SelectMany(question => question.Answers).Where(a => a.IsCorrect).Select(a => a.Id).ToArray();
            return new Lesson_ValidateQuizResponseDto
            {
                CorrectAnswerIds = correctAnswerIds
            };
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<LessonDto> UpdateLesson(int id, Lesson_UpdateRequestDto requestDto)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update lesson");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var lessonQuery = _unitOfWork.Lessons.GetAll();
            lessonQuery = lessonQuery
                .Include(l => l.CourseSection)
                    .ThenInclude(l => l.Course)
                        .ThenInclude(l => l.Owner)
                .Include(l => l.Video)
                .Where(x => x.Id == id);

            var lesson = await lessonQuery.FirstOrDefaultAsync();

            if (lesson == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Lesson not found");
            }

            if (lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to update this lesson");
            }

            if (requestDto.Title != null)
            {
                lesson.Title = requestDto.Title;
            }

            if (requestDto.Order != null)
            {
                lesson.Order = requestDto.Order.Value;
            }

            if (requestDto.VideoId != null)
            {
                if (lesson.Type != LessonType.Video)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Video id can only be updated for video lesson");
                }
                var videoQuery = _unitOfWork.Videos.GetAll();
                videoQuery = videoQuery
                    .Include(v => v.File.User)
                    .Where(x => x.Id == requestDto.VideoId);
                var video = await videoQuery.FirstOrDefaultAsync();

                if (video == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "Video not found");
                }
                if (video.File.UserId != currentUser.Id)
                {
                    throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to use this video");
                }
                if (video.LessonId != null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Video is already used in another lesson");
                }
                video.LessonId = lesson.Id;
                _unitOfWork.SaveChanges();
            }

            _unitOfWork.SaveChanges();
            transaction.Commit();

            lesson = await lessonQuery.FirstOrDefaultAsync();
            return _mapper.Map<LessonDto>(lesson);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<LessonDto> DeleteLesson(int id)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to delete lesson");
        }
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var lessonQuery = _unitOfWork.Lessons.GetAll();
            lessonQuery = lessonQuery
                .Include(l => l.CourseSection)
                    .ThenInclude(l => l.Course)
                .Include(l => l.Quiz)
                    .ThenInclude(l => l.Questions)
                        .ThenInclude(l => l.Answers)
                .Include(l => l.Video)
                .Where(x => x.Id == id);

            var lesson = await lessonQuery.FirstOrDefaultAsync();

            if (lesson == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Lesson not found");
            }

            if (lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to delete this lesson");
            }

            if (lesson.Type == LessonType.Quiz)
            {
                var quiz = lesson.Quiz;
                var questions = quiz.Questions;
                var answers = questions.SelectMany(q => q.Answers);
                _unitOfWork.Answers.RemoveRange(answers);
                _unitOfWork.Questions.RemoveRange(questions);
                _unitOfWork.Quizzes.Remove(quiz);
                _unitOfWork.SaveChanges();
            }
            else if (lesson.Type == LessonType.Video)
            {
                var video = lesson.Video;
                video.LessonId = null;
                _unitOfWork.SaveChanges();
            }
            var deletedLesson = lesson;

            _unitOfWork.Lessons.Remove(lesson);
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return _mapper.Map<LessonDto>(deletedLesson);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
