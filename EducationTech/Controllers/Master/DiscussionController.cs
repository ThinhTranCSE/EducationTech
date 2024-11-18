using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Discussions;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

public class DiscussionController : BaseController
{
    private readonly IDiscussionService _discussionService;

    public DiscussionController(IDiscussionService discussionService)
    {
        _discussionService = discussionService;
    }

    [HttpPost]
    public async Task<DiscussionDto> CreateDiscussion(Discussion_CreateRequest request)
    {
        var result = await _discussionService.CreateDiscussion(request);

        return result;
    }

}
