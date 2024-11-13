using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Videos
{
    public class VideoDto : AbstractDto<Video, VideoDto>
    {
        public int Id { get; set; }
        public int? LearningObjectId { get; set; }
        public string Url { get; set; } = null!;
    }
}
