using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Comments;

namespace EducationTech.Business.Master.Interfaces;

public interface ICommentService : IService
{
    Task<CommentDto> CreateComment(Comment_CreateRequest request);
}
