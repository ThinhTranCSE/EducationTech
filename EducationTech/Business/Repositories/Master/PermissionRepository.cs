using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Master
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public override DbSet<Permission> Model => _context.Permissions;

        public PermissionRepository(EducationTechContext context) : base(context)
        {
        }


    }
}
