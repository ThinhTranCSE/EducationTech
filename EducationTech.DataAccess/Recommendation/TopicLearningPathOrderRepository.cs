using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;

namespace EducationTech.DataAccess.Recommendation;

public class TopicLearningPathOrderRepository : Repository<TopicLearningPathOrder>, ITopicLearningPathOrderRepository
{
    public TopicLearningPathOrderRepository(IMainDatabaseContext context) : base(context)
    {
    }

}
