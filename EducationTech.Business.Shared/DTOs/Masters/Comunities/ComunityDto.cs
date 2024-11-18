using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Masters.Discussions;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Comunities;

public class ComunityDto : AbstractDto<Comunity, ComunityDto>
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public virtual ICollection<DiscussionDto> Discussions { get; set; } = new List<DiscussionDto>();
}
