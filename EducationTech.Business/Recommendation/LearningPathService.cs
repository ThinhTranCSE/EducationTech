using AutoMapper;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;
using EducationTech.Business.Shared.DTOs.Recommendation.RecommendTopics;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
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

    public async Task<OldLearningPathDto> RecommendLearningPath(int learnerId, int startUnitId, int targetUnitId)
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

        var learningPathDto = new OldLearningPathDto
        {
            LearningPath = courses
        };

        return learningPathDto;
    }

    public async Task<LearningPathDto> RecomendLearningPathSemester(int learnerId, int specialityId)
    {
        int totalCreditToGraduate = 128;
        int recommendedSemesterCount = 8;
        int averageCreditsPerSemester = totalCreditToGraduate / recommendedSemesterCount;
        int maxCreditsPerSemester = averageCreditsPerSemester + 4;


        var additionalLearnerLogInfomationLookUp = new Dictionary<int, LearnerLogInformations>();

        var learner = await _unitOfWork.Learners.Find(l => l.Id == learnerId).FirstOrDefaultAsync();

        List<string> projectCourseGroupNames = new() { "A", "B" };
        var courseGroups = await _unitOfWork.CourseGroups.GetAll()
            .Where(cg => !projectCourseGroupNames.Contains(cg.Name))
            .Include(cg => cg.Courses.Where(c => c.Specialities.Count != 0))
            .ThenInclude(c => c.Topics)
            .ThenInclude(t => t.LearningObjects)
            .ThenInclude(lo => lo.LearnerLogs)
            .ToListAsync();

        var sortedCourses = new SortedList<double, Course>(new DuplicateKeyComparer<double>());


        // đảm bảo học đủ tín chỉ tối thiểu của từng course group
        var sortedCourseOfCourseGroups = new SortedList<double, Course>(new DuplicateKeyComparer<double>());

        foreach (var course in courseGroups.SelectMany(cg => cg.Courses))
        {
            var score = CalculateCourseScore(learner, course, additionalLearnerLogInfomationLookUp);
            sortedCourseOfCourseGroups.Add(-score, course);
        }

        var minCreditsOfCourseGroups = new Dictionary<int, int>();
        foreach (var courseGroup in courseGroups)
        {
            minCreditsOfCourseGroups.Add(courseGroup.Id, courseGroup.MinCredits);
        }

        // thêm các course có simialar tốt nhất của coursegroup vào sortedCourses
        foreach (var (score, course) in sortedCourseOfCourseGroups)
        {
            if (minCreditsOfCourseGroups.Values.Sum() == 0)
            {
                break;
            }

            int courseGroupId = (int)course.CourseGroupId;

            if (minCreditsOfCourseGroups[courseGroupId] > 0)
            {
                sortedCourses.Add(score, course);
                minCreditsOfCourseGroups[courseGroupId] -= course.Credits;
            }
        }


        // đồ án tổng hợp
        var project1Course = await _unitOfWork.Courses.GetAll()
            .IncludeFullCourseInformations()
            .Where(c => c.Specialities.Any(cs => cs.SpecialityId == specialityId))
            .Where(c => c.CourseGroupId == 2)
            .FirstOrDefaultAsync();
        // đồ án đa ngành
        var project2Course = await _unitOfWork.Courses.GetAll()
            .IncludeFullCourseInformations()
            .Where(c => c.Specialities.Any(cs => cs.SpecialityId == specialityId))
            .Where(c => c.CourseGroupId == 3)
            .FirstOrDefaultAsync();
        // thực tập
        var internshipCourse = await _unitOfWork.Courses.GetAll()
            .IncludeFullCourseInformations()
            .Where(c => c.CourseCode == "CO3335")
            .FirstOrDefaultAsync();
        // đồ án chuyên ngành
        var project3Course = await _unitOfWork.Courses.GetAll()
            .IncludeFullCourseInformations()
            .Where(c => c.CourseCode == "CO4029")
            .FirstOrDefaultAsync();
        // đồ án tốt nghiệp
        var project4Course = await _unitOfWork.Courses.GetAll()
            .IncludeFullCourseInformations()
            .Where(c => c.CourseCode == "CO4337")
            .FirstOrDefaultAsync();

        var courses = await _unitOfWork.Courses.GetAll()
            .IncludeFullCourseInformations()
            //just get courses that at specialityId
            .Where(c => c.Specialities.Any(cs => cs.SpecialityId == specialityId))
            //just get courses that not in sortedCourseOfCourseGroups
            .Where(c => !sortedCourseOfCourseGroups.Values.Select(x => x.Id).Contains(c.Id))
            .Where(c => c.Id != project1Course!.Id)
            .Where(c => c.Id != project2Course!.Id)
            .Where(c => c.Id != internshipCourse!.Id)
            .Where(c => c.Id != project3Course!.Id)
            .Where(c => c.Id != project4Course!.Id)
            .ToListAsync();

        foreach (var course in courses)
        {
            var score = CalculateCourseScore(learner, course, additionalLearnerLogInfomationLookUp);
            sortedCourses.Add(-score, course);
        }


        // đảm bảo học đủ tín chỉ của từng semester

        var learnedCourseIds = new HashSet<int>();
        var learningPath = new List<SemesterCourseDto>();

        var sortedCoursesList = sortedCourses.Values.ToList();

        var specialCourseLookUp = new Dictionary<int, List<Course>>()
        {
            {5, new(){ project1Course} },
            {6, new(){ project2Course } },
            {7, new(){ internshipCourse, project3Course } },
            {8, new(){ project4Course } }
        };


        for (int semester = 1; semester <= recommendedSemesterCount; semester++)
        {
            var currentSemsesterCredits = 0;
            var semesterCourses = new List<Course>();

            // thêm các course đặc biệt vào learning path
            if (specialCourseLookUp.TryGetValue(semester, out var specialCourses))
            {
                foreach (var specialCourse in specialCourses)
                {
                    learnedCourseIds.Add(specialCourse.Id);
                    semesterCourses.Add(specialCourse);
                    currentSemsesterCredits += specialCourse.Credits;
                }
            }

            foreach (var course in sortedCoursesList)
            {
                if (averageCreditsPerSemester <= currentSemsesterCredits && currentSemsesterCredits <= maxCreditsPerSemester)
                {
                    break;
                }

                // đảm bảo học xong (hoặc đã nằm trong learning path) các course prerequisites
                if (course.Prerequisites.Any(p => !learnedCourseIds.Contains(p.PrerequisiteCourseId)))
                {
                    continue;
                }

                if (learnedCourseIds.Contains(course.Id))
                {
                    continue;
                }

                //chọn course hiện tại
                learnedCourseIds.Add(course.Id);
                semesterCourses.Add(course);
                currentSemsesterCredits += course.Credits;

            }
            learningPath.Add(new SemesterCourseDto
            {
                Semester = semester,
                Courses = _mapper.ProjectTo<Course_MinimalDto>(semesterCourses.AsQueryable()).ToList(),
                TotalCredits = currentSemsesterCredits
            });

            foreach (var course in semesterCourses)
            {
                sortedCoursesList.Remove(course);
            }
        }

        var learningPathDto = new LearningPathDto
        {
            TotalCredits = learningPath.Sum(c => c.TotalCredits),
            LearningPath = learningPath
        };

        var learningObjectDtos = learningPathDto.LearningPath.SelectMany(sc => sc.Courses.SelectMany(c => c.Topics.SelectMany(t => t.LearningObjects))).ToList();

        foreach (var learningObjectDto in learningObjectDtos)
        {
            if (additionalLearnerLogInfomationLookUp.TryGetValue(learningObjectDto.Id, out var learnerLogInformations))
            {
                learningObjectDto.LearnerLogInformations = learnerLogInformations;
            }
        }

        return learningPathDto;
    }


    // utility methods
    private const double SCORE_WHEN_NO_LEARNING_OBJECT = double.MinValue;
    private double CalculateCourseScore(Learner learner, Course course, Dictionary<int, LearnerLogInformations>? additionalLearnerLogInfomationLookUp = null)
    {
        double score = 0;
        var topics = course.Topics;
        if (topics.Count == 0)
        {
            return SCORE_WHEN_NO_LEARNING_OBJECT;
        }

        var topicScores = new Dictionary<int, double>();
        foreach (var topic in topics)
        {
            var learningObjects = topic.LearningObjects;
            if (learningObjects.Count == 0)
            {
                topicScores.Add(topic.Id, SCORE_WHEN_NO_LEARNING_OBJECT);
                continue;
            }

            var learningObjectScores = new Dictionary<int, double>();
            double topicScore = 0.0;
            foreach (var learningObject in learningObjects)
            {
                var learningObjectScore = CalculateSimilarity(learner, learningObject, additionalLearnerLogInfomationLookUp);
                topicScore += learningObjectScore;
                learningObjectScores.Add(learningObject.Id, learningObjectScore);
            }
            var LOexs = learningObjects.Where(x => x.Type == LOType.Explanatory).ToList();
            var LOevs = learningObjects.Where(x => x.Type == LOType.Evaluative).ToList();

            var sortedLearningObjects = new List<LearningObject>();
            sortedLearningObjects.AddRange(LOexs.OrderBy(l => learningObjectScores[l.Id]));
            sortedLearningObjects.AddRange(LOevs.OrderBy(l => learningObjectScores[l.Id]));

            // sort learning objects by similarity
            topic.LearningObjects = sortedLearningObjects;

            topicScores.Add(topic.Id, topicScore);
        }

        var sortedTopics = new List<RecommendTopic>();

        sortedTopics.AddRange(topics.OrderBy(t => topicScores[t.Id]));

        // sort topics by similarity
        course.Topics = sortedTopics;

        score = topicScores.Values.Sum();

        return score / course.Topics.SelectMany(t => t.LearningObjects).Count();
    }

    private double CalculateSimilarity(Learner learner, LearningObject learningObject, Dictionary<int, LearnerLogInformations>? additionalLearnerLogInfomationLookUp = null)
    {
        double ability = CalculateAbility(learner, learningObject, additionalLearnerLogInfomationLookUp);
        double difficulty = CalculateDifficulty(learningObject, additionalLearnerLogInfomationLookUp);

        return Math.Sqrt(Math.Pow(ability - difficulty, 2));
    }

    private double CalculateAbility(Learner learner, LearningObject learningObject, Dictionary<int, LearnerLogInformations>? additionalLearnerLogInfomationLookUp = null)
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

        // lấy thông tin ra ngoài
        if (additionalLearnerLogInfomationLookUp != null)
        {
            if (additionalLearnerLogInfomationLookUp.TryGetValue(learningObject.Id, out var learnerLogInformations))
            {
                learnerLogInformations.MaximumScore = maximumScore;
                learnerLogInformations.MinimumTime = minimumTime;
                learnerLogInformations.LearnerLogInformationOfSelectedLearner = new LearnerLogInformationOfSelectedLearner
                {
                    LearnerId = learner.Id,
                    ScoreObtainedByLearner = scoreObtainedByLearner,
                    TimeTakenByLearner = timeTakenByLearner,
                    AttemptByLearner = attemptByLearner
                };
            }
            else
            {
                additionalLearnerLogInfomationLookUp.Add(learningObject.Id, new LearnerLogInformations
                {
                    MaximumScore = maximumScore,
                    MinimumTime = minimumTime,
                    MeanScore = 0,
                    MeanTime = 0,
                    LearnerLogInformationOfSelectedLearner = new LearnerLogInformationOfSelectedLearner
                    {
                        LearnerId = learner.Id,
                        ScoreObtainedByLearner = scoreObtainedByLearner,
                        TimeTakenByLearner = timeTakenByLearner,
                        AttemptByLearner = attemptByLearner
                    }
                });
            }
        }

        return alpha * (scoreObtainedByLearner / maximumScore) + beta * (minimumTime - timeTakenByLearner) / minimumTime + gamma * (1 - attemptByLearner);
    }

    private double CalculateDifficulty(LearningObject learningObject, Dictionary<int, LearnerLogInformations>? additionalLearnerLogInfomationLookUp = null)
    {
        double delta = 0.5;
        double zeta = 0.3;
        double theta = 0.2;

        double maximumScore = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxScore : learningObject.LearnerLogs.Max(x => x.Score);
        double minimumTime = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxLearningTime : learningObject.LearnerLogs.Min(x => x.TimeTaken);

        double meanScore = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxScore : learningObject.LearnerLogs.Average(x => x.Score);
        double meanTime = learningObject.LearnerLogs.Count == 0 ? learningObject.MaxLearningTime : learningObject.LearnerLogs.Average(x => x.TimeTaken);

        if (additionalLearnerLogInfomationLookUp != null)
        {
            if (additionalLearnerLogInfomationLookUp.TryGetValue(learningObject.Id, out var learnerLogInformations))
            {
                learnerLogInformations.MaximumScore = maximumScore;
                learnerLogInformations.MinimumTime = minimumTime;

                learnerLogInformations.MeanScore = meanScore;
                learnerLogInformations.MeanTime = meanTime;
            }
            else
            {
                additionalLearnerLogInfomationLookUp.Add(learningObject.Id, new LearnerLogInformations
                {
                    MaximumScore = maximumScore,
                    MinimumTime = minimumTime,
                    MeanScore = meanScore,
                    MeanTime = meanTime,
                    LearnerLogInformationOfSelectedLearner = new LearnerLogInformationOfSelectedLearner
                    {
                        LearnerId = 0,
                        ScoreObtainedByLearner = 0,
                        TimeTakenByLearner = 0,
                        AttemptByLearner = 0
                    }
                });
            }
        }


        return delta * learningObject.Difficulty + zeta * (1 - (meanScore / maximumScore)) + theta * (meanTime / minimumTime);
    }
}

public static class LearningPathServiceExtensions
{
    public static IQueryable<Course> IncludeFullCourseInformations(this IQueryable<Course> query)
    {
        query = query
            .Include(c => c.Prerequisites)
            .Include(c => c.Specialities)
            .Include(c => c.Topics)
            .ThenInclude(t => t.LearningObjects)
            .ThenInclude(lo => lo.LearnerLogs);

        return query;
    }
}

public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
{

    public int Compare(TKey x, TKey y)
    {
        int result = x.CompareTo(y);

        if (result == 0)
            return 1; // Handle equality as being greater. Note: this will break Remove(key) or
                      // IndexOfKey(key) since the comparer never returns 0 to signal key equality

        return result;
    }

}
