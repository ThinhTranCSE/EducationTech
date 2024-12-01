using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class LearningPath_ProgressDto
{
    public int TotalLearningObjectCount { get; set; }
    public int PassedLearningObjectCount { get; set; }
    public ICollection<LearningObjectDto> PassedLearningObjects { get; set; } = new List<LearningObjectDto>();
}
