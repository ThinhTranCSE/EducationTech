using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Master
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public override DbSet<Course> Model => _context.Courses;

        public CourseRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
