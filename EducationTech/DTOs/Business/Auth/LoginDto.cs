using System.ComponentModel.DataAnnotations;

namespace EducationTech.DTOs.Business.Auth
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
