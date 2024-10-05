using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.RecommendationSystem.DataStructures.PathGenerations;
using EducationTech.RecommendationSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.RecommendationSystem.Implementations.PathGenerations;

public class LoPathVisitor : ILoPathVisitor
{
    private readonly ILoSuitableSelector _loSuitableSelector;
    private readonly IUnitOfWork _unitOfWork;
    public LoPathVisitor(ILoSuitableSelector loSuitableSelector, IUnitOfWork unitOfWork)
    {
        _loSuitableSelector = loSuitableSelector;
        _unitOfWork = unitOfWork;
    }
    public async Task<List<LearningPath>> SelectAllLoPaths(Learner learner, RecommendTopic startTopic, RecommendTopic targetTopic)
    {
        //using DFS to find all paths
        var paths = new List<LearningPath>();

        var initialPath = new LearningPath();

        var initialVisited = new HashSet<int>();

        await DFS(learner, startTopic, targetTopic, initialPath, initialVisited, paths);

        foreach (var path in paths)
        {
            await path.AssignSuitableLos(_loSuitableSelector, learner);
        }

        return paths;

    }

    private async Task DFS(Learner learner, RecommendTopic currentTopic, RecommendTopic targetUnit, LearningPath currentPath, HashSet<int> currentVisited, List<LearningPath> paths)
    {
        //tránh việc lặp vĩnh viễn khi có chu trình (1 node sẽ chỉ xuất hiện 1 lần trong path)
        if (currentVisited.Contains(currentTopic.Id))
        {
            return;
        }

        currentVisited = new HashSet<int>(currentVisited);
        currentVisited.Add(currentTopic.Id);

        var path = new LearningPath(currentPath);
        //visit current node

        var topicQuery = _unitOfWork.RecommendTopics.GetAll();
        topicQuery = topicQuery.Where(x => x.Id == currentTopic.Id).Include(x => x.NextTopicConjuctions).ThenInclude(x => x.NextTopic);
        currentTopic = await topicQuery.FirstOrDefaultAsync();

        //var (explanatoryLearningObject, evaluativeLearningObject) = await _loSuitableSelector.SelectSuitableLoPair(learner, currentTopic);
        path.AddLearningNode(currentTopic);

        if (currentTopic.Id == targetUnit.Id)
        {
            paths.Add(path);
            return;
        }

        if (currentTopic.NextTopicConjuctions == null || currentTopic.NextTopicConjuctions.Count == 0)
        {
            return;
        }

        foreach (var nextTopicConjuction in currentTopic.NextTopicConjuctions)
        {
            await DFS(learner, nextTopicConjuction.NextTopic, targetUnit, path, currentVisited, paths);
        }
    }


}
