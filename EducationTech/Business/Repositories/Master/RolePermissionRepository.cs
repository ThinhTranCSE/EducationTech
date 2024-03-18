using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Master
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public override DbSet<RolePermission> Model => _context.RolePermissions;
        public RolePermissionRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
