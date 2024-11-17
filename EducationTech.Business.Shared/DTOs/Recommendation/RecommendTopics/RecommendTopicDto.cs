using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;

public class RecommendTopicDto : AbstractDto<RecommendTopic, RecommendTopicDto>
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
    public int CourseId { get; set; }
    public virtual ICollection<TopicLearningPathOrderDto> TopicLearningPathOrders { get; set; } = new List<TopicLearningPathOrderDto>();
    public virtual ICollection<LearningObjectDto> LearningObjects { get; set; }
}
