using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business
{
    public class AnswerUserRepository : Repository<AnswerUser>, IAnswerUserRepository
    {
        public AnswerUserRepository(IMainDatabaseContext context) : base(context)
        {
        }

    }
}
