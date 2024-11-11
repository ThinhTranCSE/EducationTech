using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;

public class RecommendTopic_UpdateRequest : AbstractDto<RecommendTopic_UpdateRequest, RecommendTopic>
{
    public int? Order { get; set; }
    public string? Name { get; set; }
    public int? CourseId { get; set; }

    public override void Configure(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<RecommendTopic_UpdateRequest, RecommendTopic>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
