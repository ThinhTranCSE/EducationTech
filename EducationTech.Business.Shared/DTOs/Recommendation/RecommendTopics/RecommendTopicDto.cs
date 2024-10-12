using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;

public class RecommendTopicDto : AbstractDto<RecommendTopic, RecommendTopicDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CourseId { get; set; }
    public virtual ICollection<LearningObjectDto> LearningObjects { get; set; }
}
