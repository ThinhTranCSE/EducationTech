using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Shared.Utilities.Abstract;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ILearnerCollaborativeFilter : IUtils
{
    /// <summary>
    /// Trả ra n learner tương đồng nhất với learner truyền vào
    /// </summary>
    /// <param name="learner">learner cần xác định độ tương đồng</param>
    /// <param name="interestedLearners">danh sách learner cần quan tâm và so sánh với "learner" trong thuật toán (khi sử dụng cần include LearningStyle và LearningLogs)</param>
    /// <param name="n">số learner tương đồng với learner truyền vô</param>
    /// <returns>ditionary, trong đó learner là key, độ tương đồng là value</returns>
    Task<Dictionary<Learner, float>> TopNSimilarityLearners(Learner learner, IList<Learner> interestedLearners, int n = -1);
}
