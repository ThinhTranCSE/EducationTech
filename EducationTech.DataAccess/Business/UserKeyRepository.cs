using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts.Interfaces;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.DataAccess.Business;

public class UserKeyRepository : Repository<UserKey>, IUserKeyRepository
{
    public UserKeyRepository(IMainDatabaseContext context) : base(context)
    {
    }


}
