using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Master
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public override DbSet<Role> Model => _context.Roles;

        public RoleRepository(EducationTechContext context) : base(context)
        {
        }

    }
}
