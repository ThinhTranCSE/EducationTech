using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Shared.DTOs.Business.File
{
    public class File_PrepareResponseDto
    {
        public Guid SessionId { get; set; }
        public int ChunkSize { get; set; }

        public int TotalChunks { get; set; }
        public string OriginalFileName { get; set; }
    }
}
