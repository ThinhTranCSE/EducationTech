using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;

namespace EducationTech.DataAccess.Recommendation;

public class LearningStyleRepository : Repository<LearningStyle>, ILearningStyleRepository
{
    public LearningStyleRepository(IMainDatabaseContext context) : base(context)
    {
    }

}
