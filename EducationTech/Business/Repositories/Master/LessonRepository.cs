using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Master
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public override DbSet<Lesson> Model => _context.Lessons;
        public LessonRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
