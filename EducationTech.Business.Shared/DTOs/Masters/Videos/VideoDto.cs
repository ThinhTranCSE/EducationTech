using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Business.Shared.DTOs.Masters.Videos
{
    public class VideoDto : AbstractDto<Video, VideoDto>
    {
        public int Id { get; set; }
        public int? LessonId { get; set; }
        public string Url { get; set; }
    }
}
