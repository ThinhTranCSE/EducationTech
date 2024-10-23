using EducationTech.Business.Shared.DTOs.Masters.Courses;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class OldLearningPathDto
{
    public List<Course_MinimalDto> LearningPath { get; set; } = new();
}
