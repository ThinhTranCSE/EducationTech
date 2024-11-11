using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;

namespace EducationTech.Business.Recommendation.Interfaces;

public interface IRecomendTopicService : IService
{
    Task<RecommendTopicDto> CreateTopic(RecommendTopic_CreateRequest requestDto);
    Task<RecommendTopicDto> UpdateTopic(RecommendTopic_UpdateRequest requestDto, int id);
    Task<bool> DeleteTopic(int id);
}
