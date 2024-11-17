using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class TopicLearningPathOrderDto : AbstractDto<TopicLearningPathOrder, TopicLearningPathOrderDto>
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public int LearnerId { get; set; }
    public int Order { get; set; }
}
