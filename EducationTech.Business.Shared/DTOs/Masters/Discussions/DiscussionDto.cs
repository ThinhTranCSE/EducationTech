using AutoMapper;
using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Comments;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Shared.NestedSet;

namespace EducationTech.Business.Shared.DTOs.Masters.Discussions;

public class DiscussionDto : AbstractDto<Discussion, DiscussionDto>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid OwnerId { get; set; }
    public int ComunityId { get; set; }

    public ICollection<NestedSetRecursiveNodeDto<CommentDto>> Comments { get; set; } = new List<NestedSetRecursiveNodeDto<CommentDto>>();

    public override void Configure(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Discussion, DiscussionDto>()
           .ForMember(dest => dest.Comments, opt => opt.MapFrom((src, dest, _, context) =>
           {
               var comments = src.Comments;
               var commentDtos = comments.Select(c => context.Mapper.Map<CommentDto>(c));
               return commentDtos.ToTrees();
           }));
    }
}
