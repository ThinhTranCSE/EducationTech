using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Master
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public override DbSet<UserRole> Model => _context.UserRoles;

        public UserRoleRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
