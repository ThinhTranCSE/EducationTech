using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Recommendation.CourseGroups;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Recommendation;

public class CourseGroupController : BaseController
{
    private readonly ICourseGroupService _courseGroupService;

    public CourseGroupController(ICourseGroupService courseGroupService)
    {
        _courseGroupService = courseGroupService;
    }

    [HttpGet]
    public async Task<List<CourseGroupDto>> GetAll()
    {
        var courseGroups = await _courseGroupService.GetAll();
        return courseGroups;
    }

    [HttpPost]
    public async Task<CourseGroupDto> CreateCourseGroup([FromBody] CourseGroup_CreateRequestDto requestDto)
    {
        var courseGroup = await _courseGroupService.CreateCourseGroup(requestDto);
        return courseGroup;
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteCourseGroup(int id)
    {
        var result = await _courseGroupService.DeleteCourseGroup(id);
        return result;
    }
}
