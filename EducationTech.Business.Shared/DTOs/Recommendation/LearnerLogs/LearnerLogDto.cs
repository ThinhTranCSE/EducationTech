using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearnerLogs;

public class LearnerLogDto : AbstractDto<LearnerLog, LearnerLogDto>
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public int LearningObjectId { get; set; }
    public int Score { get; set; }
    public int Attempt { get; set; }
    public int TimeTaken { get; set; }
}
