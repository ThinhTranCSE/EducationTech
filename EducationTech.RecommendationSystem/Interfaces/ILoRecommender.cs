
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ILoRecommender
{
    /// <summary>
    /// TopN Learning Objects recommendation cho learner
    /// </summary>
    /// <param name="learner">learner cần recommend</param>
    /// <param name="interestedLearners">(chỉ sử dụng trong SimilaRatingRecommend) danh sách learner cần quan tâm và so sánh với "learner" trong thuật toán (khi sử dụng cần include LearningStyle và LearningLogs)</param>
    /// <param name="interestedLearningObjects">danh sách các LOs cần quan tâm trong thuật toán (khi sử dụng cần include LearnerLogs và Learner)</param>
    /// <param name="numberOfRecommendations">số LOs gợi ý</param>
    /// <returns></returns>
    Task<List<LearningObject>> RecommendTopNLearningObjects(Learner learner, List<Learner> interestedLearners, List<LearningObject> interestedLearningObjects, int numberOfRecommendations);
}
