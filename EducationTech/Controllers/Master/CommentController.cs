using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Comments;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

public class CommentController : BaseController
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<CommentDto> CreateComment(Comment_CreateRequest request)
    {
        var result = await _commentService.CreateComment(request);

        return result;
    }
}
