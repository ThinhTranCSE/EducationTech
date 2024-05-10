using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Users
{
    public class UserDto : Abstracts.AbstractDto<User, UserDto>
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = "";

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
