using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.CourseGroups;

public class CourseGroupDto : Abstracts.AbstractDto<CourseGroup, CourseGroupDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MinCredits { get; set; }

}
