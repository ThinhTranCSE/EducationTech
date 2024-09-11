using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.RecommendationSystem.Interfaces;

namespace EducationTech.RecommendationSystem.Implementations.LoRecommenders;

public class SimilarUserRatingLoRecommender : ILoRecommender
{
    private readonly ILearnerCollaborativeFilter _learnerCollaborativeFilter;
    private readonly ILearnerRepository _learnerRepository;
    private readonly ILearningObjectRepository _learningObjectRepository;


    public SimilarUserRatingLoRecommender(ILearnerCollaborativeFilter learnerCollaborativeFilter, ILearnerRepository learnerRepository, ILearningObjectRepository learningObjectRepository)
    {
        _learnerCollaborativeFilter = learnerCollaborativeFilter;
        _learnerRepository = learnerRepository;
        _learningObjectRepository = learningObjectRepository;
    }
    public async Task<List<LearningObject>> RecommendTopNLearningObjects(Learner learner, int numberOfRecommendations)
    {
        //_similarLearnersLookUpTable = await _learnerCollaborativeFilter.TopNSimilarityLearners(learner, 5);

        //List<LearningObject> learningObjects = (await _learningObjectRepository.Get()).ToList();

        //var sortedLearningObjects = new PriorityQueue<LearningObject, double>(numberOfRecommendations);

        throw new NotImplementedException();




    }
    public float LearningObjectScoreCalculation(Learner learner, LearningObject learningObject)
    {
        throw new NotImplementedException();

    }

}
