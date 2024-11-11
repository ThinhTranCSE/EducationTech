using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Recommendation.CourseGroups;

namespace EducationTech.Business.Recommendation.Interfaces;

public interface ICourseGroupService : IService
{
    Task<List<CourseGroupDto>> GetAll();
    Task<CourseGroupDto> CreateCourseGroup(CourseGroup_CreateRequestDto requestDto);
    Task<bool> DeleteCourseGroup(int id);
}
