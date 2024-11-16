using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Recommendation;

public class LearningPathController : BaseController
{
    private readonly ILearningPathService _learningPathService;
    private readonly ISessionService _sessionService;

    public LearningPathController(ILearningPathService learningPathService, ISessionService sessionService)
    {
        _learningPathService = learningPathService;
        _sessionService = sessionService;
    }

    //[HttpPost("OldRecommend")]
    //public async Task<OldLearningPathDto> RecommendLearningPath([FromBody] LearningPath_OldRequestDto request)
    //{
    //    var learningPath = await _learningPathService.RecommendLearningPath(request.LearnerId, request.StartUnitId, request.TargetUnitId);
    //    return learningPath;
    //}

    [HttpGet("Recommend")]
    public async Task<LearningPathDto> RecommendLearningPath()
    {
        var learnerId = _sessionService.CurrentUser?.Learner?.Id;
        if (learnerId == null)
        {
            throw new Exception("You are not Learner");
        }

        var specialityId = _sessionService.CurrentUser?.Learner?.SpecialityId;

        var learningPath = await _learningPathService.RecomendLearningPathSemester(learnerId.Value, specialityId.Value);

        return learningPath;
    }

}
