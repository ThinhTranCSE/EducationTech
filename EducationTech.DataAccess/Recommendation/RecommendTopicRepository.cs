using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;

namespace EducationTech.DataAccess.Recommendation;

public class RecommendTopicRepository : Repository<RecommendTopic>, IRecommendTopicRepository
{
    public RecommendTopicRepository(IMainDatabaseContext context) : base(context)
    {
    }

}
