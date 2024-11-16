namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths.SaveRequest;

public class LearningPath_SaveRequest_Topic
{
    public int Id { get; set; }
    public ICollection<LearningPath_SaveRequest_LearningObject> LearningObjects { get; set; } = new List<LearningPath_SaveRequest_LearningObject>();
}
