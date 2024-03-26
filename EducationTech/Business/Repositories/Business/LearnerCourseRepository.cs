using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class LearnerCourseRepository : Repository<LearnerCourse>, ILearnerCourseRepository
    {
        public override DbSet<LearnerCourse> Model => _context.LearnerCourses;
        public LearnerCourseRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
