using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

public class CourseController : BaseController
{
    private readonly ICourseService _courseService;
    public CourseController(EducationTechContext context, IAuthService authService, ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    //[Cache(300)]
    public async Task<Course_GetResponseDto> GetCourses([FromQuery] Course_GetRequestDto requestDto)
    {
        var coursesResponse = await _courseService.GetPaginatedData(requestDto, requestDto.Offset, requestDto.Limit, null);
        return coursesResponse;
    }

    [HttpGet("{id}")]
    public async Task<CourseDto> GetCourseById(int id, [FromQuery] Course_GetByIdRequestDto requestDto)
    {
        var course = await _courseService.GetCourseById(requestDto, id);
        return course;
    }

    [HttpPost]
    [Authorize(Policy = "UploadCourse")]
    public async Task<CourseDto> CreateCourse([FromBody] Course_CreateRequestDto requestDto)
    {
        var course = await _courseService.CreateCourse(requestDto);
        return course;
    }

    [HttpPatch("{id}")]
    [Authorize(Policy = "UpdateCourse")]
    public async Task<CourseDto> UpdateCourse(int id, [FromBody] Course_UpdateRequestDto requestDto)
    {
        var course = await _courseService.UpdateCourse(requestDto, id);
        return course;
    }

    [HttpPost("{id}/Buy")]
    [Authorize]
    public async Task<CourseDto> BuyCourse(int id, [FromBody] Course_BuyRequestDto requestDto)
    {
        var course = await _courseService.BuyCourse(requestDto, id);
        return course;
    }

}
