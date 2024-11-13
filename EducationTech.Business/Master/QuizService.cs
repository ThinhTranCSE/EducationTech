
using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;

class QuizService : IQuizService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QuizService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<QuizDto> CreateQuiz(Quiz_CreateRequest requestDto)
    {
        var quiz = _mapper.Map<Quiz>(requestDto);
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.Quizzes.Add(quiz);
            _unitOfWork.SaveChanges();
            transaction.Commit();
            return _mapper.Map<QuizDto>(quiz);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> DeleteQuiz(int id)
    {
        var quiz = _unitOfWork.Quizzes.GetAll().FirstOrDefault(x => x.Id == id);

        if (quiz == null)
        {
            throw new Exception("Quiz not found");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            foreach (var question in quiz.Questions)
            {
                _unitOfWork.Answers.RemoveRange(question.Answers);
                _unitOfWork.Questions.Remove(question);
            }
            _unitOfWork.Quizzes.Remove(quiz);
            _unitOfWork.SaveChanges();
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

    }

    public async Task<QuizDto> UpdateQuiz(Quiz_UpdateRequest requestDto, int id)
    {
        var quiz = _unitOfWork.Quizzes.GetAll().FirstOrDefault(x => x.Id == id);

        if (quiz == null)
        {
            throw new Exception("Quiz not found");
        }

        _mapper.Map(requestDto, quiz);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.SaveChanges();
            transaction.Commit();
            return _mapper.Map<QuizDto>(quiz);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
