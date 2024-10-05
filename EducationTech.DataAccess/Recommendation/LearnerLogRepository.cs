using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;

namespace EducationTech.DataAccess.Recommendation;

public class LearnerLogRepository : Repository<LearnerLog>, ILearnerLogRepository
{
    public LearnerLogRepository(IMainDatabaseContext context) : base(context)
    {
    }

}
