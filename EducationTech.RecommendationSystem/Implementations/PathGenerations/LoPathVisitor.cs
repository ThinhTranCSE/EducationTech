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
        var paths = await FindAllPaths(startTopic.Id, targetTopic.Id);

        //var initialPath = new LearningPath();
        //var initialVisited = new HashSet<int>();
        //await DFS(learner, startTopic, targetTopic, initialPath, initialVisited, paths);

        var losSelected = new Dictionary<int, (LearningObject, LearningObject)>();
        foreach (var path in paths)
        {
            await path.AssignSuitableLos(_loSuitableSelector, learner, losSelected);
        }

        return paths;

    }

    private async Task<List<LearningPath>> FindAllPaths(int startTopicId, int targetTopicId)
    {
        var paths = await _unitOfWork.GraphClient.Cypher
            .Match("(src:Topic {Id: $sourceId }), (target:Topic {Id: $targetId })")
            .WithParam("sourceId", startTopicId)
            .WithParam("targetId", targetTopicId)
            .Call("apoc.path.expand(src, '', '', 0, 20)")
            .Yield("path")
            .Where("last(nodes(path)).Id = target.Id")
            //.Return<PathsResultBolt>("path")
            .Return<List<int>>("[node IN nodes(path) | node.Id] AS pathNodes")
            .ResultsAsync;

        var allPaths = paths.ToList();

        var distinctTopicIds = allPaths.SelectMany(x => x).Distinct().ToHashSet();

        var topics = await _unitOfWork.RecommendTopics.GetAll().Where(x => distinctTopicIds.Contains(x.Id)).ToListAsync();

        var topicLookUpTable = topics.ToDictionary(x => x.Id);

        var result = allPaths.Select(x =>
        {
            var path = new LearningPath();
            foreach (var topicId in x)
            {
                path.AddLearningNode(topicLookUpTable[topicId]);
            }

            return path;
        }).ToList();

        return result;
    }


}
