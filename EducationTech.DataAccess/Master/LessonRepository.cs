using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Master
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public override DbSet<Lesson> Model => _context.Lessons;
        public LessonRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
