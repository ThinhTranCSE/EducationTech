using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Shared.DTOs.Business.File
{
    public class File_PrepareResponseDto
    {
        public UploadedFile File { get; set; }
        public IEnumerable<File_ChunkInfomationDto> Chunks { get; set; }
    }
}
