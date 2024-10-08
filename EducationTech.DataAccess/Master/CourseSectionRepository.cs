using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;

namespace EducationTech.DataAccess.Master
{
    public class CourseSectionRepository : Repository<CourseSection>, ICourseSectionRepository
    {
        public CourseSectionRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
