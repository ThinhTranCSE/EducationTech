using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ILoSuitableSelector
{
    /// <summary>
    /// select pair of suitable LOs có similarỉty tốt nhất với Learner
    /// </summary>
    /// <param name="learner"></param>
    /// <param name="searchedTopic"></param>
    /// <returns>pair (LOex, LOev)</returns>
    Task<(LearningObject, LearningObject)> SelectSuitableLoPair(Learner learner, RecommendTopic searchedTopic);
}
