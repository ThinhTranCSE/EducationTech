
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ILoRecommender
{
    Task<List<LearningObject>> RecommendTopNLearningObjects(Learner learner, int numberOfRecommendations);
    //float LearningObjectScoreCalculation(Learner learner, LearningObject learningObject);
}
