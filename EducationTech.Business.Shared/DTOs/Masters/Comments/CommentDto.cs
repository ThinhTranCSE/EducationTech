using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Users;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Shared.NestedSet;

namespace EducationTech.Business.Shared.DTOs.Masters.Comments;

public class CommentDto : AbstractDto<Comment, CommentDto>, INestedSetNode
{
    public int Id { get; set; }
    public string Content { get; set; }
    public Guid OwnerId { get; set; }
    public int Left { get; set; }
    public int Right { get; set; }
    public int? RepliedCommentId { get; set; }
    public int DiscussionId { get; set; }

    public UserDto Owner { get; set; }
}
