using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Recommendation.Specialities;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.CourseSpecialities;

public class CourseSpecialityDto : AbstractDto<CourseSpeciality, CourseSpecialityDto>
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int SpecialityId { get; set; }
    public virtual SpecialityDto Speciality { get; set; }
}
