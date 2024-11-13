using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;

public class LearningObject_UpdateRequest : AbstractDto<LearningObject_UpdateRequest, LearningObject>
{
    public int? TopicId { get; set; }
    public string? Title { get; set; }
    public int? Order { get; set; }
    public int? Difficulty { get; set; }
    public int? MaxScore { get; set; }
    public int? MaxLearningTime { get; set; }

    public override void Configure(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<LearningObject_UpdateRequest, LearningObject>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
