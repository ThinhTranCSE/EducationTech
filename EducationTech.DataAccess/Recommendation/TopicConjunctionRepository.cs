using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Recommendation;

public class TopicConjunctionRepository : Repository<TopicConjunction>, ITopicConjunctionRepository
{
    public TopicConjunctionRepository(EducationTechContext context) : base(context)
    {
    }

    public override DbSet<TopicConjunction> Model => _context.TopicConjunctions;
}
