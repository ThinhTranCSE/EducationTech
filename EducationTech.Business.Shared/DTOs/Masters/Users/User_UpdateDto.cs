using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.Shared.DTOs.Masters.Users
{
    public class User_UpdateDto
    {
        //[Required]
        //public string? Username { get; set; }
        //public string? Password { get; set; } = null!;
        [Phone]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int[]? RoleIds { get; set; }
    }
}