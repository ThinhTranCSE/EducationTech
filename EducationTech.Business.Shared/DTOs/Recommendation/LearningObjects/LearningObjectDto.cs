using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;

public class LearningObjectDto : AbstractDto<LearningObject, LearningObjectDto>
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public string Title { get; set; }
    public LOType Type { get; set; }
}
