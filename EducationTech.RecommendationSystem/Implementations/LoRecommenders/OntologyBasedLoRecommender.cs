using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
using EducationTech.RecommendationSystem.Interfaces;

namespace EducationTech.RecommendationSystem.Implementations.LoRecommenders;

public class OntologyBasedLoRecommender : ILoRecommender
{
    private readonly ILearningObjectRepository _learningObjectRepository;

    public OntologyBasedLoRecommender(ILearningObjectRepository learningObjectRepository)
    {
        _learningObjectRepository = learningObjectRepository;
    }
    public async Task<List<LearningObject>> RecommendTopNLearningObjects(Learner learner, int numberOfRecommendations)
    {
        List<LearningObject> learningObjects = (await _learningObjectRepository.Get()).ToList();

        var sortedLearningObjects = new PriorityQueue<LearningObject, float>(numberOfRecommendations);

        foreach (var learningObject in learningObjects)
        {
            float score = LearningObjectScoreCalculation(learner, learningObject);
            sortedLearningObjects.Enqueue(learningObject, score);
        }

        var result = new List<LearningObject>();
        for (int i = 0; i < numberOfRecommendations; i++)
        {
            if (sortedLearningObjects.TryDequeue(out var lo, out var priority))
            {
                result.Add(lo);
            }
            else break;
        }

        return result;
    }

    public float LearningObjectScoreCalculation(Learner learner, LearningObject learningObject)
    {
        float score = 0;
        int positiveWithRuleCounter = 0;

        //1. Intuitive rule
        positiveWithRuleCounter = 0;
        if (learningObject.Format == Format.Text)
        {
            positiveWithRuleCounter++;
        }
        if (learningObject.LearningResourceType == LearningResourceType.Diagram)
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.SemanticDensity == SemanticDensity.Medium ||
            learningObject.SemanticDensity == SemanticDensity.High ||
            learningObject.SemanticDensity == SemanticDensity.VeryHigh
            )
        {
            positiveWithRuleCounter++;
        }
        score += positiveWithRuleCounter * learner.LearningStyle.Intuitive;


        //2. Sensitive rule
        positiveWithRuleCounter = 0;
        if (learningObject.Format == Format.Video || learningObject.Format == Format.Application)
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.LearningResourceType == LearningResourceType.Figure ||
            learningObject.LearningResourceType == LearningResourceType.Slide ||
            learningObject.LearningResourceType == LearningResourceType.Table ||
            learningObject.LearningResourceType == LearningResourceType.Graph ||
            learningObject.LearningResourceType == LearningResourceType.Index ||
            learningObject.LearningResourceType == LearningResourceType.Exam ||
            learningObject.LearningResourceType == LearningResourceType.ProblemStatement
            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.SemanticDensity == SemanticDensity.VeryLow ||
            learningObject.SemanticDensity == SemanticDensity.Low ||
            learningObject.SemanticDensity == SemanticDensity.Medium
            )
        {
            positiveWithRuleCounter++;
        }
        score += positiveWithRuleCounter * learner.LearningStyle.Sensing;


        //3. Visual rule
        positiveWithRuleCounter = 0;
        if (
            learningObject.Format == Format.Video ||
            learningObject.Format == Format.Application ||
            learningObject.Format == Format.Image
            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.LearningResourceType == LearningResourceType.Graph ||
            learningObject.LearningResourceType == LearningResourceType.Index ||
            learningObject.LearningResourceType == LearningResourceType.Diagram ||
            learningObject.LearningResourceType == LearningResourceType.Figure ||
            learningObject.LearningResourceType == LearningResourceType.Slide ||
            learningObject.LearningResourceType == LearningResourceType.ProblemStatement
            )
        {
            positiveWithRuleCounter++;
        }
        score += positiveWithRuleCounter * learner.LearningStyle.Visual;


        //4. Verbal rule
        positiveWithRuleCounter = 0;
        if (
            learningObject.Format == Format.Audio ||
            learningObject.Format == Format.Text ||
            learningObject.Format == Format.Form
            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.LearningResourceType == LearningResourceType.Questionnaire ||
            learningObject.LearningResourceType == LearningResourceType.Table ||
            learningObject.LearningResourceType == LearningResourceType.Exam ||
            learningObject.LearningResourceType == LearningResourceType.Experiment ||
            learningObject.LearningResourceType == LearningResourceType.SelfAssessment ||
            learningObject.LearningResourceType == LearningResourceType.Lecture
            )
        {
            positiveWithRuleCounter++;
        }
        score += positiveWithRuleCounter * learner.LearningStyle.Verbal;

        //5. Active rule
        positiveWithRuleCounter = 0;
        if (learningObject.AggregationLevel == AggregationLevel.Level1)
        {
            positiveWithRuleCounter++;
        }
        if (learningObject.Format == Format.Application)
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.LearningResourceType == LearningResourceType.Exercise ||
            learningObject.LearningResourceType == LearningResourceType.Simulation ||
            learningObject.LearningResourceType == LearningResourceType.Questionnaire ||
            learningObject.LearningResourceType == LearningResourceType.ProblemStatement ||
            learningObject.LearningResourceType == LearningResourceType.SelfAssessment

            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.InteractivityType == InteractivityType.Active ||
            learningObject.InteractivityType == InteractivityType.Mixed
            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.InteractivityLevel == InteractivityLevel.Medium ||
            learningObject.InteractivityLevel == InteractivityLevel.High ||
            learningObject.InteractivityLevel == InteractivityLevel.VeryHigh
            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.SemanticDensity == SemanticDensity.Medium ||
            learningObject.SemanticDensity == SemanticDensity.High ||
            learningObject.SemanticDensity == SemanticDensity.VeryHigh
            )
        {
            positiveWithRuleCounter++;
        }
        score += positiveWithRuleCounter * learner.LearningStyle.Active;

        //6. Reflective rule
        positiveWithRuleCounter = 0;
        if (learningObject.AggregationLevel == AggregationLevel.Level2)
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.Format == Format.Video ||
            learningObject.Format == Format.Audio ||
            learningObject.Format == Format.Text ||
            learningObject.Format == Format.Image ||
            learningObject.Format == Format.Form

            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.LearningResourceType == LearningResourceType.Graph ||
            learningObject.LearningResourceType == LearningResourceType.Index ||
            learningObject.LearningResourceType == LearningResourceType.Diagram ||
            learningObject.LearningResourceType == LearningResourceType.Slide ||
            learningObject.LearningResourceType == LearningResourceType.Table ||
            learningObject.LearningResourceType == LearningResourceType.NarrativeText ||
            learningObject.LearningResourceType == LearningResourceType.Experiment ||
            learningObject.LearningResourceType == LearningResourceType.Lecture

            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.InteractivityType == InteractivityType.Mixed ||
            learningObject.InteractivityType == InteractivityType.Expositive
                                             )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.InteractivityLevel == InteractivityLevel.VeryLow ||
            learningObject.InteractivityLevel == InteractivityLevel.Low ||
            learningObject.InteractivityLevel == InteractivityLevel.Medium
            )
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.SemanticDensity == SemanticDensity.VeryLow ||
            learningObject.SemanticDensity == SemanticDensity.Low
            )
        {
            positiveWithRuleCounter++;
        }
        score += positiveWithRuleCounter * learner.LearningStyle.Reflective;

        //7. Sequential rule
        positiveWithRuleCounter = 0;
        if (learningObject.Structure == Structure.Linear)
        {
            positiveWithRuleCounter++;
        }
        if (learningObject.AggregationLevel == AggregationLevel.Level1)
        {
            positiveWithRuleCounter++;
        }
        if (
            learningObject.LearningResourceType == LearningResourceType.NarrativeText ||
            learningObject.LearningResourceType == LearningResourceType.Exam
            )
        {
            positiveWithRuleCounter++;
        }
        score += positiveWithRuleCounter * learner.LearningStyle.Sequential;

        //8. Global rule
        positiveWithRuleCounter = 0;
        if (learningObject.Structure == Structure.Networked)
        {
            positiveWithRuleCounter++;
        }
        if (learningObject.AggregationLevel == AggregationLevel.Level2)
        {
            positiveWithRuleCounter++;
        }
        if (learningObject.LearningResourceType == LearningResourceType.ProblemStatement)
        {
            positiveWithRuleCounter++;
        }
        score += positiveWithRuleCounter * learner.LearningStyle.Global;

        return -score;
    }
}
