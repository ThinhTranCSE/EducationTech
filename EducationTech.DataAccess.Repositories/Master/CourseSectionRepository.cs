using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Master
{
    public class CourseSectionRepository : Repository<CourseSection>, ICourseSectionRepository
    {
        public override DbSet<CourseSection> Model => _context.CourseSections;
        public CourseSectionRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
