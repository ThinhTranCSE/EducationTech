using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.CourseGroups;

public class CourseGroup_CreateRequestDto : Abstracts.AbstractDto<CourseGroup_CreateRequestDto, CourseGroup>
{
    public string Name { get; set; }
    public int MinCredits { get; set; }
}
