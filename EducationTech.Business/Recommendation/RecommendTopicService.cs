using AutoMapper;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Recommendation;

public class RecommendTopicService : IRecomendTopicService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RecommendTopicService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RecommendTopicDto> CreateTopic(RecommendTopic_CreateRequest requestDto)
    {
        var recommendTopic = _mapper.Map<RecommendTopic>(requestDto);

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.RecommendTopics.Add(recommendTopic);
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return _mapper.Map<RecommendTopicDto>(recommendTopic);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }

    }

    public Task<bool> DeleteTopic(int id)
    {
        var recommendTopic = _unitOfWork.RecommendTopics.GetAll()
            .Include(t => t.LearningObjects)
                .ThenInclude(lo => lo.LearnerLogs)
            .Include(t => t.LearningObjects)
                .ThenInclude(lo => lo.Video)
            .Include(t => t.LearningObjects)
                .ThenInclude(lo => lo.Quiz)
            .FirstOrDefault(x => x.Id == id);

        if (recommendTopic == null)
        {
            throw new Exception("Recommend topic not found");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.LearnerLogs.RemoveRange(recommendTopic.LearningObjects.SelectMany(x => x.LearnerLogs));
            _unitOfWork.LearningObjects.RemoveRange(recommendTopic.LearningObjects);

            _unitOfWork.RecommendTopics.Remove(recommendTopic);
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<RecommendTopicDto> UpdateTopic(RecommendTopic_UpdateRequest requestDto, int id)
    {
        var recommendTopic = _unitOfWork.RecommendTopics.GetAll().FirstOrDefault(x => x.Id == id);

        if (recommendTopic == null)
        {
            throw new Exception("Recommend topic not found");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            recommendTopic = _mapper.Map(requestDto, recommendTopic);
            _unitOfWork.SaveChanges();
            transaction.Commit();

            return _mapper.Map<RecommendTopicDto>(recommendTopic);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }
}
