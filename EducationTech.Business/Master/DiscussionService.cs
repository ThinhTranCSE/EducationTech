using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Discussions;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Master;

public class DiscussionService : IDiscussionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _sessionService;

    public DiscussionService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService sessionService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionService = sessionService;
    }

    public async Task<DiscussionDto> CreateDiscussion(Discussion_CreateRequest request)
    {
        var userId = _sessionService.CurrentUser?.Id;

        if (userId == null)
        {
            throw new Exception("You have not loged in");
        }

        var discussion = _mapper.Map<Discussion>(request);
        discussion.OwnerId = userId.Value;

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            _unitOfWork.Discussions.Add(discussion);
            _unitOfWork.SaveChanges();

            transaction.Commit();

            return _mapper.Map<DiscussionDto>(discussion);

        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<DiscussionDto> GetDiscussionById(int id)
    {
        var discussion = await _unitOfWork.Discussions.GetAll()
            .Include(d => d.Owner)
            .Include(d => d.Comments)
                .ThenInclude(c => c.Owner)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (discussion == null)
        {
            throw new Exception("Discussion not found");
        }

        return _mapper.Map<DiscussionDto>(discussion);
    }
}
