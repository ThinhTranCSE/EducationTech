using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;

public class RecommendTopic_CreateRequest : AbstractDto<RecommendTopic_CreateRequest, RecommendTopic>
{
    public string Name { get; set; }
    public int CourseId { get; set; }
    public int Order { get; set; }
}
