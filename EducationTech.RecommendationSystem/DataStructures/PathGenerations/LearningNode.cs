using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.RecommendationSystem.DataStructures.PathGenerations;

public class LearningNode
{
    public RecommendTopic Topic { get; }
    public LearningObject ExplanatoryLearningObject { get; }
    public LearningObject EvaluativeLearningObject { get; }

    public LearningNode(RecommendTopic topic, LearningObject explanatoryLearningObject, LearningObject evaluativeLearningObject)
    {
        Topic = topic;
        ExplanatoryLearningObject = explanatoryLearningObject;
        EvaluativeLearningObject = evaluativeLearningObject;
    }
}
