using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.RecommendationSystem.Interfaces;

namespace EducationTech.RecommendationSystem.Implementations.LoRecommenders;

public class SimilarUserRatingLoRecommender : ILoRecommender
{
    private readonly ILearnerCollaborativeFilter _learnerCollaborativeFilter;

    public SimilarUserRatingLoRecommender(ILearnerCollaborativeFilter learnerCollaborativeFilter)
    {
        _learnerCollaborativeFilter = learnerCollaborativeFilter;
    }
    public async Task<List<LearningObject>> RecommendTopNLearningObjects(Learner learner, List<Learner> interestedLearners, List<LearningObject> interestedLearningObjects, int numberOfRecommendations)
    {
        var similarLearnersLookUpTable = await _learnerCollaborativeFilter.TopNSimilarityLearners(learner, interestedLearners);
        var similarLearnerIds = similarLearnersLookUpTable.Select(x => x.Key.Id).ToHashSet();

        var learningObjects = interestedLearningObjects.Where(x => x.LearnerLogs.Any(ll => similarLearnerIds.Contains(ll.LearnerId))); ;

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
