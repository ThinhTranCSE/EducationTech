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

    [HttpGet("Load")]
    public async Task<LearningPathDto?> LoadLearningPath()
    {
        return await _learningPathService.LoadLearningPath();
    }

    [HttpPost("Save")]
    public async Task<bool> SaveLearningPath([FromBody] LearningPath_SaveRequest request)
    {
        return await _learningPathService.SaveLearningPath(request);
    }

    [HttpGet("Progress")]
    public async Task<LearningPath_ProgressDto?> LoadLearningPathProgress()
    {
        return await _learningPathService.LoadLearningPathProgress();
    }

    [HttpPost("Demo/AdjustLogToChangeSimilarity")]
    public async Task<bool> AdjustLogToChangeSimilarity([FromBody] AdjustLogToChangeSimilarityRequest request)
    {
        return await _learningPathService.AdjustLogToChangeSimilarity(request.LearnerId, request.CourseTitle, request.TargetSimilarity);
    }

    [HttpPut("Demo/MakeLearnerUseLearningPath")]
    public async Task<bool> MakeLearnerUseLearningPath(int learnerId)
    {
        return await _learningPathService.MakeLearnerUseLearningPath(learnerId);
    }

}
