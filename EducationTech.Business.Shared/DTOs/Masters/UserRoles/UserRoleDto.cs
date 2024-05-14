using EducationTech.Business.Shared.DTOs.Masters.Roles;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.UserRoles
{
    public class UserRoleDto : Abstracts.AbstractDto<UserRole, UserRoleDto>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public virtual RoleDto Role { get; set; }
    }
}
