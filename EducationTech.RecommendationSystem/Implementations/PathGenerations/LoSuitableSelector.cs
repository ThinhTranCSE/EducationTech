using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
using EducationTech.RecommendationSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.RecommendationSystem.Implementations.PathGenerations;

public class LoSuitableSelector : ILoSuitableSelector
{
    private const int TOP_N_SEQUENCES = 20;
    private readonly ILoSequenceRecommender _loSequenceRecommender;

    private readonly IUnitOfWork _unitOfWork;

    public LoSuitableSelector(ILoSequenceRecommender loSequenceRecommender, IUnitOfWork unitOfWork)
    {
        _loSequenceRecommender = loSequenceRecommender;
        _unitOfWork = unitOfWork;
    }
    public async Task<(LearningObject, LearningObject)> SelectSuitableLoPair(Learner learner, RecommendTopic searchedTopic)
    {
        var frequentSequences = await _loSequenceRecommender.RecommendTopNLearningObjectSequences(learner, searchedTopic);

        var topNSequences = frequentSequences.Where(x => x.Sequence.Count != 0).OrderByDescending(x => 0.2 * x.Support + 0.8 * x.Sequence.Count).Take(TOP_N_SEQUENCES).ToList();

        var learningObjectAppearIds = new HashSet<int>();
        foreach (var sequence in topNSequences)
        {
            foreach (var loId in sequence.Sequence)
            {
                learningObjectAppearIds.Add(loId[0]);
            }
        }

        var learningObjectQuery = _unitOfWork.LearningObjects.GetAll();
        learningObjectQuery = learningObjectQuery.Include(x => x.LearnerLogs).Where(x => learningObjectAppearIds.Contains(x.Id));

        var appearLearningObjects = await learningObjectQuery.ToListAsync();


        //list các LOs xuất hiện trong top N sequences được sorted theo độ similar với learner
        var explainatoryLearningObjects = new SortedList<double, LearningObject>();
        var evaluativeLearningObjects = new SortedList<double, LearningObject>();

        foreach (var learningObject in appearLearningObjects)
        {
            double similarity = CalculateSimilarity(learner, learningObject);
            if (learningObject.Type == LOType.Explanatory)
            {
                explainatoryLearningObjects.Add(similarity, learningObject);
            }
            else if (learningObject.Type == LOType.Evaluative)
            {
                evaluativeLearningObjects.Add(similarity, learningObject);
            }
        }

        var explainatoryLearningObject = explainatoryLearningObjects.Count != 0 ? explainatoryLearningObjects.First().Value : null;
        var evaluativeLearningObject = evaluativeLearningObjects.Count != 0 ? evaluativeLearningObjects.First().Value : null;
        return (explainatoryLearningObject, evaluativeLearningObject);
    }

    private double CalculateSimilarity(Learner learner, LearningObject learningObject)
    {
        double ability = CalculateAbility(learner, learningObject);
        double difficulty = CalculateDifficulty(learningObject);

        return Math.Sqrt(Math.Pow(ability - difficulty, 2));
    }

    private double CalculateAbility(Learner learner, LearningObject learningObject)
    {
        double alpha = 0.4;
        double beta = 0.3;
        double gamma = 0.3;

        double maximumScore = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxScore : learningObject.LearnerLogs.Max(x => x.Score);
        double minimumTime = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxLearningTime : learningObject.LearnerLogs.Min(x => x.TimeTaken);

        double scoreObtainedByLearner = 0;
        double timeTakenByLearner = learningObject.MaxLearningTime;
        double attemptByLearner = 0;

        var learnerLog = learningObject.LearnerLogs.FirstOrDefault(x => x.LearnerId == learner.Id);
        if (learnerLog != null)
        {
            scoreObtainedByLearner = learnerLog.Score;
            timeTakenByLearner = learnerLog.TimeTaken;
            attemptByLearner = learnerLog.Attempt;
        }

        return alpha * (scoreObtainedByLearner / maximumScore) + beta * (minimumTime - timeTakenByLearner) / minimumTime + gamma * (1 - attemptByLearner);
    }

    private double CalculateDifficulty(LearningObject learningObject)
    {
        double delta = 0.5;
        double zeta = 0.3;
        double theta = 0.2;

        double maximumScore = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxScore : learningObject.LearnerLogs.Max(x => x.Score);
        double minimumTime = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxLearningTime : learningObject.LearnerLogs.Min(x => x.TimeTaken);

        double meanScore = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxScore : learningObject.LearnerLogs.Average(x => x.Score);
        double meanTime = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxLearningTime : learningObject.LearnerLogs.Average(x => x.TimeTaken);

        return delta * learningObject.Difficulty + zeta * (1 - (meanScore / maximumScore)) + theta * (meanTime / minimumTime);
    }
}
