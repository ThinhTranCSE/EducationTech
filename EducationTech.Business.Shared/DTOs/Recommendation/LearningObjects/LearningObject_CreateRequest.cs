using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;

public class LearningObject_CreateRequest : AbstractDto<LearningObject, LearningObject_CreateRequest>
{
    public int TopicId { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public int Difficulty { get; set; }
    public int MaxScore { get; set; }
    public int MaxLearningTime { get; set; }
    public LOType Type { get; set; }
    public int? VideoId { get; set; }
}
