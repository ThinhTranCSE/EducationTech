namespace EducationTech.Business.DTOs.Business.File
{
    public class File_UploadChunkRequestBodyDto
    {
        public string ChunkName { get; set; }
        public long ChunkSize { get; set; }

        public IFormFile ChunkFormFile { get; set; }

    }
}
