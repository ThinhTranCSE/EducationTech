using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace EducationTech.Controllers.Master
{
    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;
        public CourseController(EducationTechContext context, IAuthService authService, ICourseService courseService) : base(context, authService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [Cache(300)]
        public async Task<Course_GetResponseDto> GetCourses([FromQuery]Course_GetRequestDto requestDto) 
        {
            var coursesResponse = await _courseService.GetPaginatedData(requestDto, requestDto.Offset, requestDto.Limit, null, CurrentUser);
            return coursesResponse;
        }

        [HttpGet("{id}")]
        public async Task<CourseDto> GetCourseById(int id, [FromQuery]Course_GetByIdRequestDto requestDto)
        {
            var course = await _courseService.GetCourseById(requestDto, id, CurrentUser);
            return course;
        }

    }
}
