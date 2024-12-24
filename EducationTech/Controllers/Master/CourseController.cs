using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Comunities;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core.Contexts;
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
    public async Task<CourseDto> CreateCourse([FromBody] Course_CreateRequestDto requestDto)
    {
        var course = await _courseService.CreateCourse(requestDto);
        return course;
    }

    [HttpPut("{id}")]
    public async Task<CourseDto> UpdateCourse(int id, [FromBody] Course_UpdateRequestDto requestDto)
    {
        var course = await _courseService.UpdateCourse(requestDto, id);
        return course;
    }

    [HttpGet("{courseId}/Comunity")]
    public async Task<ComunityDto> GetComunity(int courseId)
    {
        var comunity = await _courseService.GetComnunity(courseId);
        return comunity;
    }

    [HttpGet("Recent")]
    public async Task<Course_GetResponseDto> GetRecentLearningCourses([FromQuery] int limit = 3)
    {
        var courses = await _courseService.GetRecentLearningCourses(limit);
        return courses;
    }

    [HttpGet("Popular")]
    public async Task<Course_GetResponseDto> GetPopularCourse([FromQuery] int limit = 5)
    {
        var courses = await _courseService.GetPopularCourse(limit);
        return courses;
    }

}
