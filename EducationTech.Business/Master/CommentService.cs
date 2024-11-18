using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Comments;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Shared.NestedSet;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Master;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _sessionService;
    public CommentService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService sessionService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionService = sessionService;
    }

    public async Task<CommentDto> CreateComment(Comment_CreateRequest request)
    {
        var userId = _sessionService.CurrentUser?.Id;

        if (userId == null)
        {
            throw new Exception("You have not loged in");
        }

        var comment = _mapper.Map<Comment>(request);

        comment.OwnerId = userId.Value;

        if (request.RepliedCommentId != null)
        {
            var repliedComment = await _unitOfWork.Comments.GetAll().FirstOrDefaultAsync(x => x.Id == request.RepliedCommentId);

            if (repliedComment == null)
            {
                throw new Exception("Replied comment not found");
            }

            _unitOfWork.Comments.AddNode(repliedComment.Right - 1, comment);
        }
        else
        {
            var maxRight = await _unitOfWork.Comments.GetAll()
                .Where(c => c.DiscussionId == request.DiscussionId)
                .MaxAsync(c => c.Right);

            _unitOfWork.Comments.AddNode(maxRight, comment);
        }

        _unitOfWork.SaveChanges();

        return _mapper.Map<CommentDto>(comment);

    }
}
