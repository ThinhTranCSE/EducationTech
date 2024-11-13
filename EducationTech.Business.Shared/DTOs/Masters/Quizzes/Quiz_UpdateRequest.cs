using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Quizzes
{
    public class Quiz_UpdateRequest : AbstractDto<Quiz, Quiz_UpdateRequest>
    {
        public int? TimeLimit { get; set; }

        public override void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Quiz_UpdateRequest, Quiz>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
