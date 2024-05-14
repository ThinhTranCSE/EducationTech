using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class PermissionSeeder : Seeder
    {
        public PermissionSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            PermissionType[] permissionTypes = (PermissionType[])Enum.GetValues(typeof(PermissionType));
            foreach (PermissionType permissionType in permissionTypes)
            {
                string permissionName = permissionType.ToString();
                if (_context.Roles.Any(p => p.Name == permissionName))
                {
                    continue;
                }
                Permission permission = new Permission
                {
                    Name = permissionName
                };
                _context.Permissions.Add(permission);
            }
            _context.SaveChanges();
        }
    }
}
