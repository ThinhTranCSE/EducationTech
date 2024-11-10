//using EducationTech.Business.Business.Interfaces;
//using EducationTech.Business.Master.Interfaces;
//using EducationTech.Business.Shared.DTOs.Masters.CourseSections;
//using EducationTech.Controllers.Abstract;
//using EducationTech.DataAccess.Core.Contexts;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace EducationTech.Controllers.Master;

//[Authorize]
//public class CourseSectionController : BaseController
//{
//    private readonly ICourseSectionService _courseSectionService;
//    public CourseSectionController(EducationTechContext context, IAuthService authService, ICourseSectionService courseSectionService)
//    {
//        _courseSectionService = courseSectionService;
//    }

//    [HttpPost]
//    public async Task<CourseSectionDto> CreateCourseSection([FromBody] CourseSection_CreateRequestDto requestDto)
//    {
//        var courseSection = await _courseSectionService.CreateCourseSection(requestDto);
//        return courseSection;
//    }

//    [HttpPatch("{id}")]
//    public async Task<CourseSectionDto> UpdateCourseSection(int id, [FromBody] CourseSection_UpdateRequestDto requestDto)
//    {
//        var courseSection = await _courseSectionService.UpdateCourseSection(id, requestDto);
//        return courseSection;
//    }
//}
