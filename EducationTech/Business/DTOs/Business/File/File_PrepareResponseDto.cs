using EducationTech.Business.Models.Master;

namespace EducationTech.Business.DTOs.Business.File
{
    public class File_PrepareResponseDto
    {
        public UploadedFile File { get; set; }
        public IEnumerable<File_ChunkInfomationDto> Chunks { get; set; }
    }
}
