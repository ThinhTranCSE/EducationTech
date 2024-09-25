using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.RecommendationSystem.DataStructures.PathGenerations;
using EducationTech.RecommendationSystem.Interfaces;

namespace EducationTech.RecommendationSystem.Implementations.PathGenerations;

public class LearningPathRecommender : ILearningPathRecommender
{
    private readonly ILoPathVisitor _loPathVisitor;

    public LearningPathRecommender(ILoPathVisitor loPathVisitor)
    {
        _loPathVisitor = loPathVisitor;
    }
    public async Task<List<LearningObject>> RecommendLearningPath(Learner learner, RecommendTopic startUnit, RecommendTopic targetUnit)
    {
        var paths = await _loPathVisitor.SelectAllLoPaths(learner, startUnit, targetUnit);

        if (paths.Count == 0)
        {
            return new List<LearningObject>();
        }

        paths = paths.OrderBy(p => CalculatePathScore(learner, p)).ToList();

        var selectedPath = paths.First();

        var result = new List<LearningObject>();

        foreach (var node in selectedPath.LearningNodes)
        {
            result.Add(node.ExplanatoryLearningObject);
            result.Add(node.EvaluativeLearningObject);
        }

        return result;
    }

    private double CalculatePathScore(Learner learner, LearningPath path)
    {
        return path.Time;
    }

}
