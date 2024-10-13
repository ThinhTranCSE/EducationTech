namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class LearningPath_RequestDto
{
    public int LearnerId { get; set; }
    public int StartUnitId { get; set; }
    public int TargetUnitId { get; set; }
}
