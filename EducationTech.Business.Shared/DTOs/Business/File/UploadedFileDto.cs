using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Images;
using EducationTech.Business.Shared.DTOs.Masters.Videos;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Storage.Enums;

namespace EducationTech.Business.Shared.DTOs.Business.File
{
    public class UploadedFileDto : AbstractDto<UploadedFile, UploadedFileDto>
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public long Size { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPublic { get; set; }
        public Guid UserId { get; set; }
        public FileType FileType { get; set; }
        public ICollection<VideoDto> Videos { get; set; } = new List<VideoDto>();
        public ICollection<ImageDto> Images { get; set; } = new List<ImageDto>();

    }
}
