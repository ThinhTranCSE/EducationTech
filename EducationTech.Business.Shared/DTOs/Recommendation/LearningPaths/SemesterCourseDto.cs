using EducationTech.Business.Shared.DTOs.Masters.Courses;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class SemesterCourseDto
{
    public int Semester { get; set; }
    public List<Course_MinimalDto> Courses { get; set; } = new List<Course_MinimalDto>();
    public int TotalCredits { get; set; }
}
