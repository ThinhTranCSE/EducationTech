using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Master
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public override DbSet<Course> Model => _context.Courses;

        public CourseRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
