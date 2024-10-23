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

    [HttpPost("OldRecommend")]
    public async Task<OldLearningPathDto> RecommendLearningPath([FromBody] LearningPath_OldRequestDto request)
    {
        var learningPath = await _learningPathService.RecommendLearningPath(request.LearnerId, request.StartUnitId, request.TargetUnitId);
        return learningPath;
    }

    [HttpPost("Recommend")]
    public async Task<LearningPathDto> RecommendLearningPath([FromBody] LearningPath_RequestDto request)
    {


        var learningPath = await _learningPathService.RecomendLearningPathSemester(request.LearnerId, request.SpecialityId);

        return learningPath;
    }

}
