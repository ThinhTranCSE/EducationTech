using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EducationTech.Business.Shared.DTOs.Business.File
{
    public class File_UploadChunkRequestBodyDto
    {
        [Required]
        public Guid SessionId { get; set; }
        [Required]
        public int Index { get; set; }
        [Required]
        public IFormFile ChunkFormFile { get; set; }

    }
}
