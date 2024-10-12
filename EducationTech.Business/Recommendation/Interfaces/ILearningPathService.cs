using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

namespace EducationTech.Business.Recommendation.Interfaces;

public interface ILearningPathService : IService
{
    Task<LearningPathDto> RecommendLearningPath(int learnerId, int startUnitId, int targetUnitId);
}
