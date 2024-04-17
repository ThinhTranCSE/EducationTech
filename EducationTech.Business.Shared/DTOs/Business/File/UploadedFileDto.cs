using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Master;
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
    }
}
