namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths.SaveRequest;

public class LearningPath_SaveRequest_Course
{
    public int Id { get; set; }
    public ICollection<LearningPath_SaveRequest_Topic> Topics { get; set; } = new List<LearningPath_SaveRequest_Topic>();
}
