using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Discussions;

public class DiscussionDto : AbstractDto<Discussion, DiscussionDto>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid OwnerId { get; set; }
    public int ComunityId { get; set; }
}
