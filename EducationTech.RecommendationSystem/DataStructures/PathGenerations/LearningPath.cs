using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.RecommendationSystem.Interfaces;

namespace EducationTech.RecommendationSystem.DataStructures.PathGenerations;

public class LearningPath
{
    public List<LearningNode> LearningNodes { get; }
    public double Score { get; set; }
    public double Time { get; set; }

    public LearningPath()
    {
        LearningNodes = new List<LearningNode>();
        Score = 0;
        Time = 0;
    }

    public LearningPath(LearningPath learningPath)
    {
        LearningNodes = new List<LearningNode>(learningPath.LearningNodes);
        Score = learningPath.Score;
        Time = learningPath.Time;
    }

    public void AddLearningNode(LearningNode learningNode, double nodeScore, double nodeTime)
    {
        LearningNodes.Add(learningNode);
        Score += nodeScore;
        Time += nodeTime;
    }

    //public void AddLearningNode(RecommendTopic topic, LearningObject explanatoryLearningObject, LearningObject evaluativeLearningObject)
    //{
    //    var explanatoryLearningObjectScore = explanatoryLearningObject.LearnerLogs.Count == 0 ? 0 : explanatoryLearningObject.LearnerLogs.Average(x => x.Score);
    //    var explanatoryLearningObjectTime = explanatoryLearningObject.LearnerLogs.Count == 0 ? explanatoryLearningObject.MaxLearningTime : explanatoryLearningObject.LearnerLogs.Average(x => x.TimeTaken);
    //    var evaluativeLearningObjectScore = evaluativeLearningObject.LearnerLogs.Count == 0 ? 0 : evaluativeLearningObject.LearnerLogs.Average(x => x.Score);
    //    var evaluativeLearningObjectTime = evaluativeLearningObject.LearnerLogs.Count == 0 ? evaluativeLearningObject.MaxLearningTime : evaluativeLearningObject.LearnerLogs.Average(x => x.TimeTaken);

    //    LearningNodes.Add(new LearningNode(topic, explanatoryLearningObject, evaluativeLearningObject));
    //    Score += explanatoryLearningObjectScore + evaluativeLearningObjectScore;
    //    Time += explanatoryLearningObjectTime + evaluativeLearningObjectTime;
    //}

    public void AddLearningNode(RecommendTopic topic)
    {
        LearningNodes.Add(new LearningNode { Topic = topic });
    }

    public async Task AssignSuitableLos(ILoSuitableSelector loSuitableSelector, Learner learner)
    {
        foreach (var learningNode in LearningNodes)
        {
            var (explanatoryLearningObject, evaluativeLearningObject) = await loSuitableSelector.SelectSuitableLoPair(learner, learningNode.Topic);
            learningNode.ExplanatoryLearningObject = explanatoryLearningObject;
            learningNode.EvaluativeLearningObject = evaluativeLearningObject;

            var explanatoryLearningObjectScore = explanatoryLearningObject.LearnerLogs.Count == 0 ? 0 : explanatoryLearningObject.LearnerLogs.Average(x => x.Score);
            var explanatoryLearningObjectTime = explanatoryLearningObject.LearnerLogs.Count == 0 ? explanatoryLearningObject.MaxLearningTime : explanatoryLearningObject.LearnerLogs.Average(x => x.TimeTaken);
            var evaluativeLearningObjectScore = evaluativeLearningObject.LearnerLogs.Count == 0 ? 0 : evaluativeLearningObject.LearnerLogs.Average(x => x.Score);
            var evaluativeLearningObjectTime = evaluativeLearningObject.LearnerLogs.Count == 0 ? evaluativeLearningObject.MaxLearningTime : evaluativeLearningObject.LearnerLogs.Average(x => x.TimeTaken);

            Score += explanatoryLearningObjectScore + evaluativeLearningObjectScore;
            Time += explanatoryLearningObjectTime + evaluativeLearningObjectTime;
        }

    }
}
