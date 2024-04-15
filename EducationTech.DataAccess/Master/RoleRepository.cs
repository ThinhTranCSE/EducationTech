using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Master
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public override DbSet<Role> Model => _context.Roles;

        public RoleRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
