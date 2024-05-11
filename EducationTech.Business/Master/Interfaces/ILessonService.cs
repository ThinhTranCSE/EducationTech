using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Lessons;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Master.Interfaces
{
    public interface ILessonService : IService
    {
        Task<LessonDto> GetLessonById(int id, User? currentUser);
    }
}
