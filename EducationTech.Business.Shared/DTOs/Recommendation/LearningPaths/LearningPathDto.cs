namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class LearningPathDto
{
    public int TotalCredits { get; set; }
    public List<SemesterCourseDto> LearningPath { get; set; } = new();
}
