namespace EducationTech.Business.Shared.DTOs.Business.File
{
    public class File_ChunkInfomationDto
    {
        public long ChunkSize { get; set; }
        public int Index { get; set; }
        public double Progress { get; set; }
        public bool IsSessionCompleted { get; set; }
    }
}