using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;

namespace EducationTech.DataAccess.Recommendation;

public class TopicConjunctionRepository : Repository<TopicConjunction>, ITopicConjunctionRepository
{
    public TopicConjunctionRepository(IMainDatabaseContext context) : base(context)
    {
    }

}
