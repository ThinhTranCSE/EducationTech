using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public override DbSet<Topic> Model => _context.Topics;
        public TopicRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
