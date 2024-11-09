using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.Specialities;

public class SpecialityDto : AbstractDto<Speciality, SpecialityDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
}
