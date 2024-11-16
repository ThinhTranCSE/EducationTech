namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths.SaveRequest;

public class LearningPath_SaveRequest_Semester
{
    public int Semester { get; set; }
    public ICollection<LearningPath_SaveRequest_Course> Courses { get; set; } = new List<LearningPath_SaveRequest_Course>();
}
