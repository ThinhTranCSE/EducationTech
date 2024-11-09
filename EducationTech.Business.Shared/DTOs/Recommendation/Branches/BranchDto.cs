using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Recommendation.Specialities;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.Branches;

public class BranchDto : AbstractDto<Branch, BranchDto>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<SpecialityDto> Specialities { get; set; } = new List<SpecialityDto>();
}
