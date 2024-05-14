using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Roles
{
    public class RoleDto : Abstracts.AbstractDto<Role, RoleDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
