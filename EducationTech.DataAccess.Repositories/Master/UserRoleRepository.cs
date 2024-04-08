using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Master
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public override DbSet<UserRole> Model => _context.UserRoles;

        public UserRoleRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
