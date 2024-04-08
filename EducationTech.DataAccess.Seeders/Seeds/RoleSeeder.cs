
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Shared.Enums;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class RoleSeeder : Seeder
    {
        public RoleSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    RoleType[] roleTypes = (RoleType[])Enum.GetValues(typeof(RoleType));
                    foreach (RoleType roleType in roleTypes)
                    {
                        string roleName = roleType.ToString();
                        if (_context.Roles.Any(r => r.Name == roleName))
                        {
                            continue;
                        }
                        Role role = new Role
                        {
                            Name = roleName
                        };
                        _context.Roles.Add(role);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

        }
    }
}
