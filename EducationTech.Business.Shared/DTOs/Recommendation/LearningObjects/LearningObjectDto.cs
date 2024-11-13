using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;

namespace EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;

public class LearningObjectDto : AbstractDto<LearningObject, LearningObjectDto>
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public LOType Type { get; set; }
    public int Difficulty { get; set; }
    public int MaxScore { get; set; }
    public int MaxLearningTime { get; set; }
    public LearnerLogInformations LearnerLogInformations { get; set; }
}

public class LearnerLogInformations
{
    public double MaximumScore { get; set; }
    public double MinimumTime { get; set; }
    public double MeanScore { get; set; }
    public double MeanTime { get; set; }

    public LearnerLogInformationOfSelectedLearner LearnerLogInformationOfSelectedLearner { get; set; }
}

public class LearnerLogInformationOfSelectedLearner
{
    public int LearnerId { get; set; }
    public double ScoreObtainedByLearner { get; set; }
    public double TimeTakenByLearner { get; set; }
    public double AttemptByLearner { get; set; }
}
