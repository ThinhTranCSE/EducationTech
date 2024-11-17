using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;
using EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Shared.DTOs.Masters.Courses;

public class Course_MinimalDto : AbstractDto<Course, Course_MinimalDto>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    public virtual ICollection<CourseLearningPathOrderDto> CourseLearningPathOrders { get; set; } = new List<CourseLearningPathOrderDto>();
    public virtual ICollection<RecommendTopicDto> Topics { get; set; } = new List<RecommendTopicDto>();
    public virtual ICollection<PrerequisiteCourseDto> Prerequisites { get; set; } = new List<PrerequisiteCourseDto>();
}
