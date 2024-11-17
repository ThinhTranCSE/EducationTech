using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;

public class CourseLearningPathOrderDto : AbstractDto<CourseLearningPathOrder, CourseLearningPathOrderDto>
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int LearnerId { get; set; }
    public int Order { get; set; }
    public int Semester { get; set; }
}
