namespace EducationTech.Business.Shared.DTOs.Recommendation.LearnerLogs;

public class LearnerLog_CreateRequest
{
    public int LearningObjectId { get; set; }
    public int Score { get; set; }
    public int TimeTaken { get; set; }
}
