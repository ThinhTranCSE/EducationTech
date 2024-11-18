using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Comments;

public class Comment_CreateRequest : AbstractDto<Comment, Comment_CreateRequest>
{
    public string Content { get; set; }
    public int? RepliedCommentId { get; set; }
    public int DiscussionId { get; set; }
}
