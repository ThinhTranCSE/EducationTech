using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.Storage;

namespace EducationTech.Business.Shared.DTOs.Masters.Images;

public class ImageDto : AbstractDto<Image, ImageDto>
{
    public int Id { get; set; }
    public string Url { get; set; }
    public Guid FileId { get; set; }
    public override void Configure(IMapperConfigurationExpression cfg)
    {
        string hostName = GlobalReference.Instance.HostName;
        string scheme = GlobalReference.Instance.HostScheme;
        cfg.CreateMap<Image, ImageDto>()
            .ForMember(x => x.Url, opt => opt.MapFrom(x => $"{scheme}://{hostName}/{x.Url}"))
            .ReverseMap();
    }
}

