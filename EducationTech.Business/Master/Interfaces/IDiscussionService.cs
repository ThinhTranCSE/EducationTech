using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Discussions;

namespace EducationTech.Business.Master.Interfaces;

public interface IDiscussionService : IService
{
    Task<DiscussionDto> CreateDiscussion(Discussion_CreateRequest request);
    Task<DiscussionDto> GetDiscussionById(int id);
}
