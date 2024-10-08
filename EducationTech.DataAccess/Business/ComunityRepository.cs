using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business
{
    public class ComunityRepository : Repository<Comunity>, IComunityRepository
    {
        public ComunityRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
