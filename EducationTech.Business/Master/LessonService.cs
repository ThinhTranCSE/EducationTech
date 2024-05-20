using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EducationTech.Business.Master
{
    public class LessonService : ILessonService
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepositoy;
        private readonly IAnswerUserRepository _answerUserRepository;
        private readonly ILearnerCourseRepository _learnerCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseSectionRepository _courseSectionRepository;
        private readonly IVideoRepository _videoRepository;
        private readonly IMapper _mapper;

        public LessonService(
            ITransactionManager transactionManager,
            ILessonRepository lessonRepository,
            IQuizRepository quizRepository,
            IQuestionRepository questionRepository,
            IAnswerRepository answerRepositoy,
            IAnswerUserRepository answerUserRepository,
            ILearnerCourseRepository learnerCourseRepository,
            ICourseRepository courseRepository,
            ICourseSectionRepository courseSectionRepository,
            IVideoRepository videoRepository,
            IMapper mapper)
        {
            _transactionManager = transactionManager;
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _answerRepositoy = answerRepositoy;
            _answerUserRepository = answerUserRepository;
            _learnerCourseRepository = learnerCourseRepository;
            _courseRepository = courseRepository;
            _courseSectionRepository = courseSectionRepository;
            _videoRepository = videoRepository;
            _mapper = mapper;
        }

        public async Task<LessonDto> CreateLesson(Lesson_CreateRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create lesson");
            }

            var courseSectionQuery = await _courseSectionRepository.Get();
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

            await _lessonRepository.Insert(createdLesson, true);

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

                await _quizRepository.Insert(quiz, true);

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

                await _questionRepository.Insert(questions, true);
            }
            else if (requestDto.Type == LessonType.Video)
            {
                if (requestDto.VideoId == null)
                {
                    throw new HttpException(HttpStatusCode.BadRequest, "Video id is required for video lesson");
                }
                var videoQuery = await _videoRepository.Get();
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
                await _videoRepository.Update(video, true);
            }
            else
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Invalid lesson type");
            }

            return _mapper.Map<LessonDto>(createdLesson);
        }


        public async Task<LessonDto> GetLessonById(int id, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to get lesson detail");
            }

            var query = await _lessonRepository.Get();
            query = query
                .Where(x => x.Id == id)
                .Include(x => x.Video)
                .Include(x => x.Quiz)
                    .ThenInclude(x => x.Questions)
                        .ThenInclude(x => x.Answers)
                .Include(x => x.CourseSection)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.LearnerCourses);
            var lesson = await query.FirstOrDefaultAsync();
            if (lesson == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Lesson not found");
            }
            if (lesson.CourseSection.Course.LearnerCourses.All(x => x.LearnerId != currentUser.Id) && lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "You dont have permission to view this lesson detail");
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

        public async Task<Lesson_ValidateQuizResponseDto> SubmitAnswers(Lesson_ValidateQuizRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to submit quizzes");
            }
            var quizQuery = await _quizRepository.Get();
            quizQuery = quizQuery
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(q => q.Lesson)
                    .ThenInclude(q => q.CourseSection)
                            .ThenInclude(q => q.Course)
                                .ThenInclude(q => q.LearnerCourses)
                .Where(q => q.Id == requestDto.QuizId)
                .Where(q => q.Lesson.CourseSection.Course.LearnerCourses.Any(x => x.LearnerId == currentUser.Id));

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
            var insertedAnswerUserTasks = requestDto.AnswerIds.Select(answerId => _answerUserRepository.Insert(new AnswerUser
            {
                UserId = currentUser.Id,
                AnswerId = answerId
            })).ToArray();

            Task.WaitAll(insertedAnswerUserTasks);

            _transactionManager.SaveChanges();


            var correctAnswerIds = quiz.Questions.SelectMany(question => question.Answers).Where(a => a.IsCorrect).Select(a => a.Id).ToArray();
            return new Lesson_ValidateQuizResponseDto
            {
                CorrectAnswerIds = correctAnswerIds
            };
        }

        public async Task<LessonDto> UpdateLesson(int id, Lesson_UpdateRequestDto requestDto, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update lesson");
            }

            var lessonQuery = await _lessonRepository.Get();
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
                var videoQuery = await _videoRepository.Get();
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
                await _videoRepository.Update(video, true);
            }

            await _lessonRepository.Update(lesson, true);
            lesson = await lessonQuery.FirstOrDefaultAsync();
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task<LessonDto> DeleteLesson(int id, User? currentUser)
        {
            if (currentUser == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Please login to delete lesson");
            }

            var lessonQuery = await _lessonRepository.Get();
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
                await _answerRepositoy.Delete(answers, true);
                await _questionRepository.Delete(questions, true);
                await _quizRepository.Delete(quiz, true);
            }
            else if (lesson.Type == LessonType.Video)
            {
                var video = lesson.Video;
                video.LessonId = null;
                await _videoRepository.Update(video, true);
            }

            var deletedLesson = await _lessonRepository.Delete(lesson, true);

            return _mapper.Map<LessonDto>(deletedLesson);
        }
    }
}
