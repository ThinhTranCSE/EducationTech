using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.Shared.DTOs.Business.Auth
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public ICollection<int> RoleIds { get; set; } = new List<int>();
        public int? SpecialityId { get; set; }
    }
}