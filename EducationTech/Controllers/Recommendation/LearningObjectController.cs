using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Recommendation;

public class LearningObjectController : BaseController
{
    private readonly ILearningObjectService _learningObjectService;

    public LearningObjectController(ILearningObjectService learningObjectService)
    {
        _learningObjectService = learningObjectService;
    }

    [HttpPost]
    public async Task<LearningObjectDto> CreateLearningObject([FromBody] LearningObject_CreateRequest request)
    {
        var learningObject = await _learningObjectService.CreateLearningObject(request);

        return learningObject;
    }

    [HttpPut("{id}")]
    public async Task<LearningObjectDto> UpdateLearningObject([FromBody] LearningObject_UpdateRequest request, int id)
    {
        var learningObject = await _learningObjectService.UpdateLearningObject(request, id);

        return learningObject;
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteLearningObject(int id)
    {
        var result = await _learningObjectService.DeleteLearningObject(id);

        return result;
    }
}
