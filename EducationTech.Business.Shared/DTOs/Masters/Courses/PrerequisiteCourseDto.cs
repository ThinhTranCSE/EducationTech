using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses;

public class PrerequisiteCourseDto : AbstractDto<PrerequisiteCourse, PrerequisiteCourseDto>
{
    public int PrerequisiteCourseId { get; set; }
}
