using AutoMapper;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;
using EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;
using EducationTech.DataAccess.Abstract;
using EducationTech.RecommendationSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Recommendation;

public class LearningPathService : ILearningPathService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILearningPathRecommender _learningPathRecommender;

    public LearningPathService(IUnitOfWork unitOfWork, IMapper mapper, ILearningPathRecommender learningPathRecommender)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _learningPathRecommender = learningPathRecommender;
    }

    public async Task<LearningPathDto> RecommendLearningPath(int learnerId, int startUnitId, int targetUnitId)
    {
        var learner = await _unitOfWork.Learners.Find(l => l.Id == learnerId).FirstOrDefaultAsync();
        var startUnit = await _unitOfWork.RecommendTopics.Find(rt => rt.Id == startUnitId).FirstOrDefaultAsync();
        var targetUnit = await _unitOfWork.RecommendTopics.Find(rt => rt.Id == targetUnitId).FirstOrDefaultAsync();

        var learningObjects = await _learningPathRecommender.RecommendLearningPath(learner, startUnit, targetUnit);

        var learningObjectIds = learningObjects.Select(lo => lo.Id).ToList();

        // Load learning objects with their relations
        var learningObjectQuery = _unitOfWork.LearningObjects.GetAll();

        learningObjectQuery = learningObjectQuery
            .Include(lo => lo.Topic)
            .ThenInclude(t => t.Course)
            .Where(lo => learningObjectIds.Contains(lo.Id));

        var learningObjectsWithRelations = await learningObjectQuery.ToListAsync();

        learningObjectsWithRelations = learningObjectsWithRelations
            .OrderBy(lo => learningObjectIds.IndexOf(lo.Id))
            .ToList();


        var courses = new List<Course_MinimalDto>();
        for (int i = 0; i < learningObjectsWithRelations.Count; i++)
        {
            var learningObject = learningObjectsWithRelations[i];
            var learingObjectDto = _mapper.Map<LearningObjectDto>(learningObject);
            if (i == 0)
            {
                var course = _mapper.Map<Course_MinimalDto>(learningObject.Topic.Course);
                course.Topics = new List<RecommendTopicDto> { _mapper.Map<RecommendTopicDto>(learningObject.Topic) };
                course.Topics.First().LearningObjects = new List<LearningObjectDto> { learingObjectDto };
                courses.Add(course);
                continue;
            }

            var previousLearningObject = learningObjectsWithRelations[i - 1];
            if (previousLearningObject.Topic.Course.Id != learningObject.Topic.Course.Id)
            {
                var course = _mapper.Map<Course_MinimalDto>(learningObject.Topic.Course);
                course.Topics = new List<RecommendTopicDto> { _mapper.Map<RecommendTopicDto>(learningObject.Topic) };
                course.Topics.First().LearningObjects = new List<LearningObjectDto> { learingObjectDto };
                courses.Add(course);
                continue;
            }

            var lastCourse = courses.Last();
            if (lastCourse.Topics.Last().Id == learningObject.Topic.Id)
            {
                lastCourse.Topics.Last().LearningObjects.Add(_mapper.Map<LearningObjectDto>(learningObject));
            }
            else
            {
                lastCourse.Topics.Add(_mapper.Map<RecommendTopicDto>(learningObject.Topic));
                lastCourse.Topics.Last().LearningObjects = new List<LearningObjectDto> { learingObjectDto };
            }
        }

        var learningPathDto = new LearningPathDto
        {
            LearningPath = courses
        };

        return learningPathDto;
    }

}
