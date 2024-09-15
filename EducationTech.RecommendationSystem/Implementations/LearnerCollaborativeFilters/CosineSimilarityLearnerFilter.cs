using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.RecommendationSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics.Tensors;

namespace EducationTech.RecommendationSystem.Implementations.LearnerCollaborativeFilters;

public class CosineSimilarityLearnerFilter : ILearnerCollaborativeFilter
{
    private readonly ILearnerRepository _learnerRepository;

    public CosineSimilarityLearnerFilter(ILearnerRepository learnerRepository)
    {
        _learnerRepository = learnerRepository;
    }
    public async Task<Dictionary<Learner, float>> TopNSimilarityLearners(Learner learner, int n)
    {
        var query = await _learnerRepository.Get();

        var learners = query
            .Include(x => x.LearningStyle)
            .Include(x => x.LearnerLogs)
            .ToList();

        //age, gender, qualification, background knowledge, learning style
        var learnerVector = new float[]
        {
            learner.Age,
            (float)learner.Gender,
            (float)learner.Qualification,
            (float)learner.BackgroundKnowledge,
            learner.LearningStyle.Intuitive,
            learner.LearningStyle.Sensing,
            learner.LearningStyle.Visual,
            learner.LearningStyle.Verbal,
            learner.LearningStyle.Sequential,
            learner.LearningStyle.Global,
            learner.LearningStyle.Active,
            learner.LearningStyle.Reflective
        };

        var similarLearners = new PriorityQueue<Learner, float>();

        foreach (var l in learners)
        {
            var lVector = new float[]
            {
                l.Age,
                (float)l.Gender,
                (float)l.Qualification,
                (float)l.BackgroundKnowledge,
                l.LearningStyle.Intuitive,
                l.LearningStyle.Sensing,
                l.LearningStyle.Visual,
                l.LearningStyle.Verbal,
                l.LearningStyle.Sequential,
                l.LearningStyle.Global,
                l.LearningStyle.Active,
                l.LearningStyle.Reflective
            };

            var negativeSimilarity = -TensorPrimitives.CosineSimilarity(learnerVector, lVector);

            similarLearners.Enqueue(l, negativeSimilarity);
        }

        var result = new Dictionary<Learner, float>();
        for (int i = 0; i < n; i++)
        {
            if (similarLearners.TryDequeue(out var l, out var negativePriority))
            {
                result.Add(l, -negativePriority);
            }
            else break;
        }

        return result;
    }
}
