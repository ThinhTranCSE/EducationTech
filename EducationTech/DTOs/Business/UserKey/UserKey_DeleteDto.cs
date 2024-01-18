using System.ComponentModel.DataAnnotations;

namespace EducationTech.DTOs.Business.UserKey
{
    public class UserKey_DeleteDto
    {
        [Required]
        public int Id { get; set; }
    }
}