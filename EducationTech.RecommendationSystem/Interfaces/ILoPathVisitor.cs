using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.RecommendationSystem.DataStructures.PathGenerations;

namespace EducationTech.RecommendationSystem.Interfaces;

public interface ILoPathVisitor
{
    Task<List<LearningPath>> SelectAllLoPaths(Learner learner, RecommendTopic startUnit, RecommendTopic targetUnit);
}
