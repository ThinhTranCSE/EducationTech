using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.LearnerLogs;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Recommendation;

public class LearningObjectService : ILearningObjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _sessionService;

    public LearningObjectService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService sessionService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionService = sessionService;
    }


    public async Task<LearningObjectDto> CreateLearningObject(LearningObject_CreateRequest request)
    {
        var learningObject = _mapper.Map<LearningObject>(request);
        learningObject.Video = null;

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.LearningObjects.Add(learningObject);
            _unitOfWork.SaveChanges();

            if (request.Type == LOType.Explanatory)
            {
                if (request.VideoId == null)
                {
                    throw new Exception("Video id is required for explanatory learning object");
                }

                var video = _unitOfWork.Videos.GetAll().FirstOrDefault(x => x.Id == request.VideoId);

                if (video == null)
                {
                    throw new Exception("Video not found");
                }

                video.LearningObjectId = learningObject.Id;

                _unitOfWork.SaveChanges();
            }

            transaction.Commit();

            return _mapper.Map<LearningObjectDto>(learningObject);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    public async Task<LearningObjectDto> UpdateLearningObject(LearningObject_UpdateRequest request, int id)
    {
        var learningObject = _unitOfWork.LearningObjects.GetAll().FirstOrDefault(x => x.Id == id);

        if (learningObject == null)
        {
            throw new Exception("Learning object not found");
        }

        _mapper.Map(request, learningObject);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.SaveChanges();

            transaction.Commit();

            return _mapper.Map<LearningObjectDto>(learningObject);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    public async Task<bool> DeleteLearningObject(int id)
    {
        var learningObject = _unitOfWork.LearningObjects.GetAll()
            .Include(lo => lo.Quiz)
                .ThenInclude(q => q.Questions)
                    .ThenInclude(q => q.Answers)
            .Include(lo => lo.Video)
            .FirstOrDefault(x => x.Id == id);

        if (learningObject == null)
        {
            throw new Exception("Learning object not found");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            if (learningObject.Video != null)
            {
                learningObject.Video.LearningObjectId = null;
            }

            if (learningObject.Quiz != null)
            {
                foreach (var question in learningObject.Quiz.Questions)
                {
                    _unitOfWork.Answers.RemoveRange(question.Answers);
                }
                _unitOfWork.Questions.RemoveRange(learningObject.Quiz.Questions);
                _unitOfWork.Quizzes.Remove(learningObject.Quiz);
            }

            _unitOfWork.LearningObjects.Remove(learningObject);

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
    public async Task<LearningObjectDto> GetLearningObjectById(int id)
    {
        var learningObject = _unitOfWork.LearningObjects.GetAll()
            .Include(lo => lo.Quiz)
                .ThenInclude(q => q.Questions)
                    .ThenInclude(q => q.Answers)
            .Include(lo => lo.Video)
            .FirstOrDefault(x => x.Id == id);

        if (learningObject == null)
        {
            throw new Exception("Learning object not found");
        }

        return _mapper.Map<LearningObjectDto>(learningObject);

    }

    public async Task<LearnerLogDto> CreateLog(LearnerLog_CreateRequest request)
    {
        var learnerId = _sessionService.CurrentUser?.Learner?.Id;

        if (learnerId == null)
        {
            throw new Exception("You are not learner");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var existingLog = _unitOfWork.LearnerLogs.GetAll().FirstOrDefault(x => x.LearningObjectId == request.LearningObjectId && x.LearnerId == learnerId);

            if (existingLog == null)
            {
                existingLog = new LearnerLog
                {
                    LearnerId = learnerId.Value,
                    LearningObjectId = request.LearningObjectId,
                    Score = request.Score,
                    TimeTaken = request.TimeTaken,
                    Attempt = 1
                };

                _unitOfWork.LearnerLogs.Add(existingLog);
            }
            else
            {
                if (existingLog.Score < request.Score)
                {
                    existingLog.Score = request.Score;
                    existingLog.TimeTaken = request.TimeTaken;
                    existingLog.Attempt++;
                }
            }

            _unitOfWork.SaveChanges();

            transaction.Commit();

            return _mapper.Map<LearnerLogDto>(existingLog);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
