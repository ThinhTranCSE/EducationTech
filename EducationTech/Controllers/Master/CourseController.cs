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
        public async Task<Course_GetResponseDto> Get([FromQuery]Course_GetRequestDto requestDto) 
        {
            var coursesResponse = await _courseService.GetPaginatedData(requestDto, requestDto.Offset, requestDto.Limit, null);
            //string domain = HttpContext.Request.Host.ToString();
            //string scheme = HttpContext.Request.Scheme;
            //foreach (var courseDto in coursesResponse.Courses)
            //{
            //    courseDto.ImageUrl = $"{scheme}://{domain}/{courseDto.ImageUrl}";
            //}
            return coursesResponse;
        }
    }
}
