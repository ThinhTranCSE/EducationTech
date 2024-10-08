using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;

namespace EducationTech.DataAccess.Master
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
