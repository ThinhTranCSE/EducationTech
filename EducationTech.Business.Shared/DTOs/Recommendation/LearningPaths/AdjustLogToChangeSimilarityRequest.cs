namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class AdjustLogToChangeSimilarityRequest
{
    public int LearnerId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public double TargetSimilarity { get; set; }
}
