using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;

namespace EducationTech.DataAccess.Recommendation;

public class LearningObjectLearningPathOrderRepository : Repository<LearningObjectLearningPathOrder>, ILearningObjectLearningPathOrderRepository
{
    public LearningObjectLearningPathOrderRepository(IMainDatabaseContext context) : base(context)
    {
    }
}

