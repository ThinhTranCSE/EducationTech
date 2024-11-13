
using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.QuizResults;
using EducationTech.Business.Shared.DTOs.Masters.Quizzes;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

class QuizService : IQuizService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _sessionService;
    public QuizService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService sessionService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionService = sessionService;
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

    public async Task<QuizResultDto> StartQuiz(int id)
    {
        var quiz = _unitOfWork.Quizzes.GetAll().FirstOrDefault(x => x.Id == id);

        if (quiz == null)
        {
            throw new Exception("Quiz not found");
        }

        var userId = _sessionService.CurrentUser?.Id;

        if (userId == null)
        {
            throw new Exception("You have not loged in");
        }


        var currentQuizResult = await _unitOfWork.QuizResults.GetAll()
            .Where(x => x.QuizId == id && x.UserId == userId)
            .Where(x => x.EndTime == null)
            .Where(x => x.StartTime.AddSeconds(quiz.TimeLimit) < DateTime.Now)
            .FirstOrDefaultAsync();

        if (currentQuizResult != null)
        {
            return _mapper.Map<QuizResultDto>(currentQuizResult);
        }

        var quizResult = new QuizResult
        {
            QuizId = id,
            UserId = userId.Value,
            StartTime = DateTime.Now
        };

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.QuizResults.Add(quizResult);
            _unitOfWork.SaveChanges();
            transaction.Commit();
            return _mapper.Map<QuizResultDto>(quizResult);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<QuizResultDto> SubmitQuiz(Quiz_SubmitQuizRequest request)
    {
        var quizResult = _unitOfWork.QuizResults.GetAll()
            .FirstOrDefault(x => x.Id == request.QuizResultId);
        if (quizResult == null)
        {
            throw new Exception("Quiz result not found");
        }

        var userId = _sessionService.CurrentUser?.Id;
        if (userId == null)
        {
            throw new Exception("You have not loged in");
        }

        if (quizResult.UserId != userId)
        {
            throw new Exception("You are not allowed to submit this quiz");
        }

        if (quizResult.EndTime != null)
        {
            throw new Exception("Quiz already submitted");
        }

        if (quizResult.StartTime.AddSeconds(quizResult.Quiz.TimeLimit) > DateTime.Now)
        {
            throw new Exception("Quiz time limit exceeded");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var score = 0;

            var answers = _unitOfWork.Answers.GetAll()
                .Include(x => x.Question)
                .Where(x => request.AnswerIds.Contains(x.Id))
                .ToList();

            if (answers.Any(answers => answers.Question.QuizId != quizResult.QuizId))
            {
                throw new Exception("Invalid answers in submit");
            }

            foreach (var answer in answers)
            {
                if (answer.IsCorrect)
                {
                    score += answer.Score;
                }

                var answerUser = new AnswerUser
                {
                    QuizResultId = quizResult.Id,
                    AnswerId = answer.Id,
                    UserId = userId.Value
                };

                _unitOfWork.AnswerUsers.Add(answerUser);
            }

            quizResult.EndTime = DateTime.Now;
            quizResult.Score = score;
            quizResult.TimeTaken = (int)(quizResult.EndTime - quizResult.StartTime).Value.TotalSeconds;

            _unitOfWork.SaveChanges();

            transaction.Commit();

            return _mapper.Map<QuizResultDto>(quizResult);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

    }
}
