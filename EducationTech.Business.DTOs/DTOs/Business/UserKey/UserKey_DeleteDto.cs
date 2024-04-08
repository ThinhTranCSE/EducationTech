using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.Shared.DTOs.Business.UserKey
{
    public class UserKey_DeleteDto
    {
        [Required]
        public int Id { get; set; }
    }
}