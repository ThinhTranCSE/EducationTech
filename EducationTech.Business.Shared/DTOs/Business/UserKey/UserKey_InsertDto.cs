using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.Shared.DTOs.Business.UserKey
{
    public class UserKey_InsertDto
    {
        [Required]
        public string PublicKey { get; set; }

        [Required]

        public Guid UserId { get; set; }
    }
}
