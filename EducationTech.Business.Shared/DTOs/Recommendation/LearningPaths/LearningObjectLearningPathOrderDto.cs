using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class LearningObjectLearningPathOrderDto : AbstractDto<LearningObjectLearningPathOrder, LearningObjectLearningPathOrderDto>
{
    public int Id { get; set; }
    public int LearningObjectId { get; set; }
    public int LearnerId { get; set; }
    public int Order { get; set; }
}
