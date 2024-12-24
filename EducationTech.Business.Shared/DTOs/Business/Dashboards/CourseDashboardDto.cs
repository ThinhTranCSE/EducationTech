using EducationTech.Business.Shared.DTOs.Recommendation.Learners;

namespace EducationTech.Business.Shared.DTOs.Business.Dashboards;

public class CourseDashboardDto
{
    public double AverageScore { get; set; }
    public IList<LearnerAverageScoreWhenLearnCourseDto> LearnerAverageScores { get; set; } = new List<LearnerAverageScoreWhenLearnCourseDto>();
}

public class LearnerAverageScoreWhenLearnCourseDto
{
    public LearnerDto Learner { get; set; }
    public double AverageScore { get; set; }
}