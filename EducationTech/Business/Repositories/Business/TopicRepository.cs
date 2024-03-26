using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public override DbSet<Topic> Model => _context.Topics;
        public TopicRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
