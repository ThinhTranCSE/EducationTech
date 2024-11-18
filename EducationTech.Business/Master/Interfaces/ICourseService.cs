using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Comunities;
using EducationTech.Business.Shared.DTOs.Masters.Courses;

namespace EducationTech.Business.Master.Interfaces
{
    public interface ICourseService : IService, IPagination<Course_GetRequestDto, Course_GetResponseDto>
    {
        Task<CourseDto> GetCourseById(Course_GetByIdRequestDto requestDto, int id);
        Task<CourseDto> CreateCourse(Course_CreateRequestDto requestDto);
        Task<CourseDto> UpdateCourse(Course_UpdateRequestDto requestDto, int id);
        Task<ComunityDto> GetComnunity(int courseId);
    }
}
