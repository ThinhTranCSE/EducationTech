using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.RecommendationSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.RecommendationSystem.Implementations.LoRecommenders;

public class SimilarUserRatingLoRecommender : ILoRecommender
{
    private readonly ILearnerCollaborativeFilter _learnerCollaborativeFilter;
    private readonly ILearnerRepository _learnerRepository;
    private readonly ILearningObjectRepository _learningObjectRepository;
    private readonly ILearnerLogRepository _learnerLogRepository;


    public SimilarUserRatingLoRecommender(ILearnerCollaborativeFilter learnerCollaborativeFilter, ILearnerRepository learnerRepository, ILearningObjectRepository learningObjectRepository, ILearnerLogRepository learnerLogRepository)
    {
        _learnerCollaborativeFilter = learnerCollaborativeFilter;
        _learnerRepository = learnerRepository;
        _learningObjectRepository = learningObjectRepository;
        _learnerLogRepository = learnerLogRepository;
    }
    public async Task<List<LearningObject>> RecommendTopNLearningObjects(Learner learner, int numberOfRecommendations)
    {
        var similarLearnersLookUpTable = await _learnerCollaborativeFilter.TopNSimilarityLearners(learner, 5);
        var similarLearnerIds = similarLearnersLookUpTable.Select(x => x.Key.Id).ToHashSet();

        var learningObjectQuery = await _learningObjectRepository.Get();
        learningObjectQuery = learningObjectQuery
            .Include(x => x.LearnerLogs)
            .ThenInclude(x => x.Learner)
            .Where(x => x.LearnerLogs.Any(ll => similarLearnerIds.Contains(ll.LearnerId)));

        var learningObjects = await learningObjectQuery.ToListAsync();

        var sortedLearningObjects = new PriorityQueue<LearningObject, float>(numberOfRecommendations);

        foreach (var learningObject in learningObjects)
        {
            var score = 0f;
            var totalSimilarity = 0f;

            foreach (var learnerLog in learningObject.LearnerLogs)
            {
                if (!similarLearnerIds.Contains(learnerLog.LearnerId))
                {
                    continue;
                }

                var similarity = similarLearnersLookUpTable[learnerLog.Learner];
                score += learnerLog.Rating * similarity;
                totalSimilarity += similarity;
            }

            if (totalSimilarity > 0)
            {
                score /= totalSimilarity;
                sortedLearningObjects.Enqueue(learningObject, score);
            }
        }

        var result = new List<LearningObject>();
        for (int i = 0; i < numberOfRecommendations; i++)
        {
            if (sortedLearningObjects.TryDequeue(out var lo, out var priority))
            {
                result.Add(lo);
            }
            else break;
        }

        return result;
    }

}
