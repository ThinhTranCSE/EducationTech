using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business;

public class TopicRepository : Repository<Topic>, ITopicRepository
{
    public TopicRepository(IMainDatabaseContext context) : base(context)
    {
    }

}
