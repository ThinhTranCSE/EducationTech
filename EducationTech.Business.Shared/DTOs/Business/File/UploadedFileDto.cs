using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Images;
using EducationTech.Business.Shared.DTOs.Masters.Videos;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Storage.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public VideoDto? Video { get; set; }
        public ImageDto? Image { get; set; }

    }
}
