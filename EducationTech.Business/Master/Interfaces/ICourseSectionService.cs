using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.CourseSections;

namespace EducationTech.Business.Master.Interfaces
{
    public interface ICourseSectionService : IService
    {
        Task<CourseSectionDto> CreateCourseSection(CourseSection_CreateRequestDto requestDto);
        Task<CourseSectionDto> UpdateCourseSection(int id, CourseSection_UpdateRequestDto requestDto);
    }
}
