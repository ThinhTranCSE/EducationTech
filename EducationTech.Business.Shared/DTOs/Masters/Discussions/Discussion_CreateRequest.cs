using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Business;

namespace EducationTech.Business.Shared.DTOs.Masters.Discussions;

public class Discussion_CreateRequest : AbstractDto<Discussion, Discussion_CreateRequest>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int ComunityId { get; set; }
}
