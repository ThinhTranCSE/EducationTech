using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Master
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public override DbSet<Permission> Model => _context.Permissions;

        public PermissionRepository(EducationTechContext context) : base(context)
        {
        }


    }
}
