using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ILearningPathRecommender
{
    /// <summary>
    ///    /// Recommend learning path for learner
    ///       /// </summary>
    ///          /// <param name="learner"></param>
    ///             /// <param name="startUnit">Khi sử dụng cần include NextTopicConjunctions -> NextTopic</param>
    ///                /// <param name="targetUnit">Khi sử dụng cần include NextTopicConjunctions -> NextTopic</param>
    ///                   /// <returns></returns>
    Task<List<LearningObject>> RecommendLearningPath(Learner learner, RecommendTopic startUnit, RecommendTopic targetUnit);
}
