using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class LearnerCourseRepository : Repository<LearnerCourse>, ILearnerCourseRepository
    {
        public override DbSet<LearnerCourse> Model => _context.LearnerCourses;
        public LearnerCourseRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
