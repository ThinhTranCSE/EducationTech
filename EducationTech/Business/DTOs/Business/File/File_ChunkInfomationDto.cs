namespace EducationTech.Business.DTOs.Business.File
{
    public class File_ChunkInfomationDto
    {
        public string ChunkName { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        public long ChunkSize { get; set; }
        public int Index { get; set; }
    }  
}
