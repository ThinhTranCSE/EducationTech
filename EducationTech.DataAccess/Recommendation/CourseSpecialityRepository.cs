using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;

namespace EducationTech.DataAccess.Recommendation;

public class CourseSpecialityRepository : Repository<CourseSpeciality>, ICourseSpecialityRepository
{
    public CourseSpecialityRepository(IMainDatabaseContext context) : base(context)
    {
    }
}
