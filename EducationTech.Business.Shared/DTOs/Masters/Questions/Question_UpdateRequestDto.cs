using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Questions;

public class Question_UpdateRequestDto : AbstractDto<Question, Question_UpdateRequestDto>
{
    public string? Content { get; set; }

    public override void Configure(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Question_UpdateRequestDto, Question>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
