using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EducationTech.Business.Master;

public class QuizService : IQuizService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionService _sessionService;
    public QuizService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ISessionService sessionService
        )
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _sessionService = sessionService;
    }

    public async Task<QuizDto> CreateQuiz(Quiz_CreateRequestDto requestDto)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to create quiz");
        }
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var lessonQuery = _unitOfWork.Lessons.GetAll();
            lessonQuery = lessonQuery
                .Include(l => l.CourseSection)
                    .ThenInclude(l => l.Course)
                        .ThenInclude(l => l.Owner)
                .Where(l => l.Id == requestDto.LessonId);

            var lesson = await lessonQuery.FirstOrDefaultAsync();

            if (lesson == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Lesson not found");
            }

            if (lesson.CourseSection.Course.OwnerId != currentUser.Id)
            {
                throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to create quiz for this lesson");
            }

            var createdQuiz = _mapper.Map<Quiz>(requestDto);

            _unitOfWork.Quizzes.Add(createdQuiz);
            _unitOfWork.SaveChanges();

            transaction.Commit();

            var quiz = await _unitOfWork.Quizzes.GetAll()
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .Where(q => q.Id == createdQuiz.Id)
                .FirstOrDefaultAsync();

            return _mapper.Map<QuizDto>(quiz);
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }

    }

    //public async Task<bool> DeleteQuiz(int id, User? currentUser)
    //{
    //    if (currentUser == null)
    //    {
    //        throw new HttpException(HttpStatusCode.Unauthorized, "Please login to delete quiz");
    //    }

    //    var quizQuery = await _quizRepository.Get();
    //    quizQuery = quizQuery
    //        .Include(q => q.Lesson)
    //            .ThenInclude(q => q.CourseSection)
    //                .ThenInclude(q => q.Course)
    //                    .ThenInclude(q => q.Owner)
    //        .Where(q => q.Id == id);

    //    var quiz = await quizQuery.FirstOrDefaultAsync();

    //    if (quiz == null)
    //    {
    //        throw new HttpException(HttpStatusCode.NotFound, "Quiz not found");
    //    }

    //    if (quiz.Lesson.CourseSection.Course.OwnerId != currentUser.Id)
    //    {
    //        throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to delete this quiz");
    //    }

    //    await _quizRepository.Delete(quiz, true);

    //    return true;
    //}

    public async Task<QuizDto> UpdateQuiz(int id, Quiz_UpdateRequestDto requestDto)
    {
        var currentUser = _sessionService.CurrentUser;
        if (currentUser == null)
        {
            throw new HttpException(HttpStatusCode.Unauthorized, "Please login to update quiz");
        }

        var quizQuery = _unitOfWork.Quizzes.GetAll();
        quizQuery = quizQuery
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .Include(q => q.Lesson)
                .ThenInclude(q => q.CourseSection)
                    .ThenInclude(q => q.Course)
                        .ThenInclude(q => q.Owner)
            .Where(q => q.Id == id);

        var quiz = await quizQuery.FirstOrDefaultAsync();

        if (quiz == null)
        {
            throw new HttpException(HttpStatusCode.NotFound, "Quiz not found");
        }
        if (quiz.Lesson.CourseSection.Course.OwnerId != currentUser.Id)
        {
            throw new HttpException(HttpStatusCode.Forbidden, "You are not allowed to update this quiz");
        }

        if (requestDto.TimeLimit != null)
        {
            quiz.TimeLimit = requestDto.TimeLimit.Value;
        }

        _unitOfWork.SaveChanges();

        quiz = await quizQuery.FirstOrDefaultAsync();
        return _mapper.Map<QuizDto>(quiz);
    }
}
