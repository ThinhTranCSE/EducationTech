using EducationTech.DataAccess.Core;
using EducationTech.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class RolePermissionSeeder : Seeder
    {
        public override int Piority => 1;
        public RolePermissionSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            var permission = _context.Permissions.ToList();
            var permissionDictionary = permission.ToDictionary(p => p.Name, p => p.Id);

            var roles = _context.Roles.ToList();

            var rolePermissionMapping = new Dictionary<RoleType, IEnumerable<PermissionType>>();
            rolePermissionMapping.Add(RoleType.Admin, new List<PermissionType> { PermissionType.AdminOnly, PermissionType.UploadCourse });
            rolePermissionMapping.Add(RoleType.Instructor, new List<PermissionType> { PermissionType.UploadCourse });

            foreach (var role in roles)
            {
                if(rolePermissionMapping.TryGetValue((RoleType)Enum.Parse(typeof(RoleType), role.Name), out var permissions))
                {
                    foreach (var permissionType in permissions)
                    {
                        var roleId = role.Id;
                        var permissionId = permissionDictionary[permissionType.ToString()];
                        if (_context.RolePermissions.Any(rp => rp.RoleId == roleId && rp.PermissionId == permissionId))
                        {
                            continue;
                        }
                        _context.RolePermissions.Add(new Entities.Master.RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        });
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}
