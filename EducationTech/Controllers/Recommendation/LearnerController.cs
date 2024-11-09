using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.Learners;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Recommendation;

public class LearnerController : BaseController
{
    private readonly ILearnerService _learnerService;

    public LearnerController(ILearnerService learnerService)
    {
        _learnerService = learnerService;
    }

    [HttpPut]
    public async Task<LearnerDto> UpdateLearner(UpdateLearnerRequest request)
    {
        var learner = await _learnerService.UpdateLearner(request);

        return learner;
    }
}
