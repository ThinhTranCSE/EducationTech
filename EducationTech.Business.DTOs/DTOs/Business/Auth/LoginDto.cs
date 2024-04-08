using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.Shared.DTOs.Business.Auth
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
