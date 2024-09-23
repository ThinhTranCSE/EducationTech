using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Recommendation;

public class RecommendTopicRepository : Repository<RecommendTopic>, IRecommendTopicRepository
{
    public RecommendTopicRepository(EducationTechContext context) : base(context)
    {
    }

    public override DbSet<RecommendTopic> Model => _context.RecommendTopics;
}
