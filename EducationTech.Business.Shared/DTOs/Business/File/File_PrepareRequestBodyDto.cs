using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.Shared.DTOs.Business.File
{
    public class File_PrepareRequestBodyDto
    {
        [Required]
        public string FileName { get; set; }
        [Required]
        public long FileSize { get; set; }
    }
}
