using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.RecommendationSystem.DataStructures.PathGenerations;

public class LearningNode
{
    public RecommendTopic Topic { get; set; }
    public LearningObject ExplanatoryLearningObject { get; set; }
    public LearningObject EvaluativeLearningObject { get; set; }
}
