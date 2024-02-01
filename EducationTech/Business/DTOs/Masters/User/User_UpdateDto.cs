using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.DTOs.Masters.User
{
    public class User_UpdateDto
    {
        [Required]
        public Guid Id { get; set; }
        public string? Username { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public string? Email { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; } = null!;
    }
}