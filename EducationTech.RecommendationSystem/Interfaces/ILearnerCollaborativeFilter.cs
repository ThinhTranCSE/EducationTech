using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ILearnerCollaborativeFilter
{
    /// <summary>
    /// Trả ra n learner tương đồng nhất với learner truyền vào
    /// </summary>
    /// <param name="learner">learner cần xác định độ tương đồng</param>
    /// <param name="n">số learner tương đồng với learner truyền vô</param>
    /// <returns>ditionary, trong đó learner là key, độ tương đồng là value</returns>
    Task<Dictionary<Learner, float>> TopNSimilarityLearners(Learner learner, int n);
}
