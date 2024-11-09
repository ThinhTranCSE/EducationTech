using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.Specialities;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Recommendation;

public class SpecialityController : BaseController
{
    private readonly ISpecialityService _specialityService;

    public SpecialityController(ISpecialityService specialityService)
    {
        _specialityService = specialityService;
    }

    [HttpPost]
    public async Task<SpecialityDto> CreateSpeciality([FromBody] CreateSpecialityRequest request)
    {
        var speciality = await _specialityService.CreateSpeciality(request);
        return speciality;
    }
}
