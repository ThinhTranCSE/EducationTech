using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Master
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public override DbSet<RolePermission> Model => _context.RolePermissions;
        public RolePermissionRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
