using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [Authorize(Policy = "UploadCourse")]
        public async Task<CourseDto> CreateCourse([FromBody]Course_CreateRequestDto requestDto)
        {
            var course = await _courseService.CreateCourse(requestDto, CurrentUser);
            return course;
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "UpdateCourse")]
        public async Task<CourseDto> UpdateCourse(int id, [FromBody]Course_UpdateRequestDto requestDto)
        {
            var course = await _courseService.UpdateCourse(requestDto, id, CurrentUser);
            return course;
        }

    }
}
