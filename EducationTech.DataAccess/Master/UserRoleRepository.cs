using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;

namespace EducationTech.DataAccess.Master;

public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{

    public UserRoleRepository(IMainDatabaseContext context) : base(context)
    {
    }

}
