using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.CourseSections;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public interface ICourseSectionService : IService
    {
        Task<CourseSectionDto> CreateCourseSection(CourseSection_CreateRequestDto requestDto, User? currentUser);
    }
}
