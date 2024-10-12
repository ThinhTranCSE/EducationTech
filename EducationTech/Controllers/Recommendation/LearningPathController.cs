using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Recommendation;

public class LearningPathController : BaseController
{
    private readonly ILearningPathService _learningPathService;

    public LearningPathController(ILearningPathService learningPathService)
    {
        _learningPathService = learningPathService;
    }

    [HttpPost("Recommend")]
    public async Task<LearningPathDto> RecommendLearningPath(int learnerId, int startUnitId, int targetUnitId)
    {
        var learningPath = await _learningPathService.RecommendLearningPath(learnerId, startUnitId, targetUnitId);
        return learningPath;
    }
}
