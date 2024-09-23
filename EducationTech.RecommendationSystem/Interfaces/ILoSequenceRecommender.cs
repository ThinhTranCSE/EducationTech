using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.RecommendationSystem.DataStructures;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ILoSequenceRecommender
{
    /// <summary>
    /// top N Learning Object sequences recommendation phù hợp với learner
    /// </summary>
    /// <param name="learner">learner cần quan tâm</param>
    /// <param name="searchedTopic">topic mà learner search</param>
    /// <returns></returns>
    Task<List<FrequentSequence<int>>> RecommendTopNLearningObjectSequences(Learner learner, RecommendTopic searchedTopic);
}
