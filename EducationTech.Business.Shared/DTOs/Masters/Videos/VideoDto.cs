using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.Storage;

namespace EducationTech.Business.Shared.DTOs.Masters.Videos
{
    public class VideoDto : AbstractDto<Video, VideoDto>
    {
        public int Id { get; set; }
        public int? LearningObjectId { get; set; }
        public string Url { get; set; } = null!;

        public override void Configure(IMapperConfigurationExpression cfg)
        {
            string hostName = GlobalReference.Instance.HostName;
            string scheme = GlobalReference.Instance.HostScheme;
            cfg.CreateMap<Video, VideoDto>()
                .ForMember(x => x.Url, opt => opt.MapFrom(x => $"{scheme}://{hostName}/{x.Url}"))
                .ReverseMap();
        }
    }
}
