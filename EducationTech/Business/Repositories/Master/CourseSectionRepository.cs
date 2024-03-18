using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Master
{
    public class CourseSectionRepository : Repository<CourseSection>, ICourseSectionRepository
    {
        public override DbSet<CourseSection> Model => _context.CourseSections;
        public CourseSectionRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
