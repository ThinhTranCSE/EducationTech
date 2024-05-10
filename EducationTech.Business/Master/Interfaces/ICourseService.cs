using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public interface ICourseService : IService, IPagination<Course_GetRequestDto, Course_GetResponseDto>
    {
        Task<CourseDto> GetCourseById(Course_GetByIdRequestDto requestDto, int id, User? currentUser);
    }
}
