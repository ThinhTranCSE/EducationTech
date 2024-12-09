using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Recommendation.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningObjects;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Recommendation;

public class LearningPathService : ILearningPathService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _sessionService;
    //private readonly ILearningPathRecommender _learningPathRecommender;

    public LearningPathService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService sessionService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionService = sessionService;
        //_learningPathRecommender = learningPathRecommender;
    }

    //public async Task<OldLearningPathDto> RecommendLearningPath(int learnerId, int startUnitId, int targetUnitId)
    //{
    //    var learner = await _unitOfWork.Learners.Find(l => l.Id == learnerId).FirstOrDefaultAsync();
    //    var startUnit = await _unitOfWork.RecommendTopics.Find(rt => rt.Id == startUnitId).FirstOrDefaultAsync();
    //    var targetUnit = await _unitOfWork.RecommendTopics.Find(rt => rt.Id == targetUnitId).FirstOrDefaultAsync();

    //    var learningObjects = await _learningPathRecommender.RecommendLearningPath(learner, startUnit, targetUnit);

    //    var learningObjectIds = learningObjects.Select(lo => lo.Id).ToList();

    //    // Load learning objects with their relations
    //    var learningObjectQuery = _unitOfWork.LearningObjects.GetAll();

    //    learningObjectQuery = learningObjectQuery
    //        .Include(lo => lo.Topic)
    //        .ThenInclude(t => t.Course)
    //        .Where(lo => learningObjectIds.Contains(lo.Id));

    //    var learningObjectsWithRelations = await learningObjectQuery.ToListAsync();

    //    learningObjectsWithRelations = learningObjectsWithRelations
    //        .OrderBy(lo => learningObjectIds.IndexOf(lo.Id))
    //        .ToList();


    //    var courses = new List<Course_MinimalDto>();
    //    for (int i = 0; i < learningObjectsWithRelations.Count; i++)
    //    {
    //        var learningObject = learningObjectsWithRelations[i];
    //        var learingObjectDto = _mapper.Map<LearningObjectDto>(learningObject);
    //        if (i == 0)
    //        {
    //            var course = _mapper.Map<Course_MinimalDto>(learningObject.Topic.Course);
    //            course.Topics = new List<RecommendTopicDto> { _mapper.Map<RecommendTopicDto>(learningObject.Topic) };
    //            course.Topics.First().LearningObjects = new List<LearningObjectDto> { learingObjectDto };
    //            courses.Add(course);
    //            continue;
    //        }

    //        var previousLearningObject = learningObjectsWithRelations[i - 1];
    //        if (previousLearningObject.Topic.Course.Id != learningObject.Topic.Course.Id)
    //        {
    //            var course = _mapper.Map<Course_MinimalDto>(learningObject.Topic.Course);
    //            course.Topics = new List<RecommendTopicDto> { _mapper.Map<RecommendTopicDto>(learningObject.Topic) };
    //            course.Topics.First().LearningObjects = new List<LearningObjectDto> { learingObjectDto };
    //            courses.Add(course);
    //            continue;
    //        }

    //        var lastCourse = courses.Last();
    //        if (lastCourse.Topics.Last().Id == learningObject.Topic.Id)
    //        {
    //            lastCourse.Topics.Last().LearningObjects.Add(_mapper.Map<LearningObjectDto>(learningObject));
    //        }
    //        else
    //        {
    //            lastCourse.Topics.Add(_mapper.Map<RecommendTopicDto>(learningObject.Topic));
    //            lastCourse.Topics.Last().LearningObjects = new List<LearningObjectDto> { learingObjectDto };
    //        }
    //    }

    //    var learningPathDto = new OldLearningPathDto
    //    {
    //        LearningPath = courses
    //    };

    //    return learningPathDto;
    //}

    public async Task<bool> SaveLearningPath(LearningPath_SaveRequest request)
    {
        var learnerId = _sessionService.CurrentUser?.Learner?.Id;

        if (learnerId == null)
        {
            throw new Exception("You are not Learner");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var courseSaves = await _unitOfWork.CourseLearningPathOrders.GetAll()
                .Where(cs => cs.LearnerId == learnerId)
                .ToListAsync();

            var topicSaves = await _unitOfWork.TopicLearningPathOrders.GetAll()
                .Where(ts => ts.LearnerId == learnerId)
                .ToListAsync();

            var learningObjectSaves = await _unitOfWork.LearningObjectLearningPathOrders.GetAll()
                .Where(ls => ls.LearnerId == learnerId)
                .ToListAsync();

            _unitOfWork.CourseLearningPathOrders.RemoveRange(courseSaves);
            _unitOfWork.TopicLearningPathOrders.RemoveRange(topicSaves);
            _unitOfWork.LearningObjectLearningPathOrders.RemoveRange(learningObjectSaves);

            _unitOfWork.SaveChanges();

            var courseOrder = 1;
            var topicOrder = 1;
            var learningObjectOrder = 1;
            foreach (var semester in request.LearningPath)
            {
                foreach (var course in semester.Courses)
                {
                    var courseSave = new CourseLearningPathOrder
                    {
                        LearnerId = learnerId.Value,
                        CourseId = course.Id,
                        Semester = semester.Semester,
                        Order = courseOrder
                    };
                    _unitOfWork.CourseLearningPathOrders.Add(courseSave);
                    courseOrder++;

                    foreach (var topic in course.Topics)
                    {
                        var topicSave = new TopicLearningPathOrder
                        {
                            LearnerId = learnerId.Value,
                            TopicId = topic.Id,
                            Order = topicOrder
                        };
                        _unitOfWork.TopicLearningPathOrders.Add(topicSave);
                        topicOrder++;

                        foreach (var learningObject in topic.LearningObjects)
                        {
                            var learningObjectSave = new LearningObjectLearningPathOrder
                            {
                                LearnerId = learnerId.Value,
                                LearningObjectId = learningObject.Id,
                                Order = learningObjectOrder
                            };
                            _unitOfWork.LearningObjectLearningPathOrders.Add(learningObjectSave);
                            learningObjectOrder++;
                        }
                    }
                }
            }

            _unitOfWork.SaveChanges();
            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<LearningPathDto?> LoadLearningPath()
    {
        var learnerId = _sessionService.CurrentUser?.Learner?.Id;

        if (learnerId == null)
        {
            throw new Exception("You are not Learner");
        }


        var query = _unitOfWork.Courses.GetAll()
            .Include(c => c.Prerequisites)
            .Include(c => c.CourseLearningPathOrders.Where(o => o.LearnerId == learnerId))
            .Include(c => c.Topics)
                .ThenInclude(t => t.TopicLearningPathOrders.Where(o => o.LearnerId == learnerId))
            .Include(c => c.Topics)
                .ThenInclude(t => t.LearningObjects)
                .ThenInclude(lo => lo.LearningObjectLearningPathOrders.Where(o => o.LearnerId == learnerId))
            .Where(c => c.CourseLearningPathOrders.Any(o => o.LearnerId == learnerId));

        var courses = await query.ToListAsync();

        if (courses.Count == 0)
        {
            return null;
        }

        var semesters = courses.GroupBy(c => c.CourseLearningPathOrders.First().Semester)
            .Select(g => new SemesterCourseDto
            {
                Semester = g.Key,
                Courses = _mapper.ProjectTo<Course_MinimalDto>(g.AsQueryable()).OrderBy(c => c.CourseLearningPathOrders.First().Order).ToList(),
                TotalCredits = g.Sum(c => c.Credits)
            })
            .OrderBy(sc => sc.Semester)
            .ToList();

        foreach (var semester in semesters)
        {
            foreach (var course in semester.Courses)
            {
                course.Topics = course.Topics.OrderBy(t => t.TopicLearningPathOrders.First().Order).ToList();
                foreach (var topic in course.Topics)
                {
                    topic.LearningObjects = topic.LearningObjects.OrderBy(lo => lo.LearningObjectLearningPathOrders.First().Order).ToList();
                }
            }
        }

        var learningPathDto = new LearningPathDto
        {
            TotalCredits = semesters.Sum(c => c.TotalCredits),
            LearningPath = semesters
        };

        return learningPathDto;
    }
    public async Task<LearningPath_ProgressDto?> LoadLearningPathProgress()
    {
        var learnerId = _sessionService.CurrentUser?.Learner?.Id;

        if (learnerId == null)
        {
            throw new Exception("You are not Learner");
        }

        var learningObjectLearningPathOrders = await _unitOfWork.LearningObjectLearningPathOrders.GetAll()
            .Where(lo => lo.LearnerId == learnerId)
            .Include(lo => lo.LearningObject)
                .ThenInclude(lo => lo.LearnerLogs.Where(ll => ll.LearnerId == learnerId))
            .ToListAsync();

        if (learningObjectLearningPathOrders.Count() == 0)
        {
            throw new Exception("Learning path not found");
        }

        int totalLearningObjectCount = learningObjectLearningPathOrders.Count();
        int passedLearningObjectCount = 0;
        var passedLearningObjects = new List<LearningObject>();
        foreach (var learningObject in learningObjectLearningPathOrders.Select(o => o.LearningObject))
        {
            if (learningObject.LearnerLogs.Count() == 0)
            {
                continue;
            }

            var leanerLog = learningObject.LearnerLogs.First();

            if (leanerLog.Score >= learningObject.MaxScore * 0.5)
            {
                passedLearningObjectCount++;
                passedLearningObjects.Add(learningObject);
            }
        }

        var result = new LearningPath_ProgressDto
        {
            TotalLearningObjectCount = totalLearningObjectCount,
            PassedLearningObjectCount = passedLearningObjectCount,
            PassedLearningObjects = _mapper.ProjectTo<LearningObjectDto>(passedLearningObjects.AsQueryable()).ToList()
        };

        return result;
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
            sortedCourseOfCourseGroups.Add(score, course);
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
            sortedCourses.Add(score, course);
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

            var semesterCourseIds = new HashSet<int>();
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
                semesterCourseIds.Add(course.Id);
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

            foreach (var courseId in semesterCourseIds)
            {
                learnedCourseIds.Add(courseId);
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

    public async Task<bool> AdjustLogToChangeSimilarity(int learnerId, string courseTitle, double targetSimilarity)
    {
        var course = await _unitOfWork.Courses.GetAll()
            .IncludeFullCourseInformations()
            .Where(c => c.Title == courseTitle)
            .FirstOrDefaultAsync();

        if (course == null)
        {
            throw new Exception("Course not found");
        }

        if (!course.Topics.Any())
        {
            throw new Exception("Course has no topic");
        }

        var learner = await _unitOfWork.Learners.Find(l => l.Id == learnerId).FirstOrDefaultAsync();

        if (learner == null)
        {
            throw new Exception("Learner not found");
        }

        using var transaction = _unitOfWork.BeginTransaction();

        try
        {
            var initialSimilarity = CalculateCourseScore(learner, course);


            if (initialSimilarity <= targetSimilarity)
            {
                return true;
            }

            var deltaCourseSimilarity = targetSimilarity - initialSimilarity;
            var deltaTopicSimilarity = deltaCourseSimilarity / course.Topics.Count;

            foreach (var topic in course.Topics)
            {
                var inititalTopicSimilarity = topic.LearningObjects.Average(lo => CalculateSimilarity(learner, lo));
                foreach (var learningObject in topic.LearningObjects)
                {
                    var deltaLearningObjectSimilarity = deltaTopicSimilarity / topic.LearningObjects.Count;
                    var currentLearnerLog = learningObject.LearnerLogs.FirstOrDefault(x => x.LearnerId == learnerId);
                    while (deltaLearningObjectSimilarity < 0)
                    {
                        var maxScore = learningObject.LearnerLogs.Max(x => x.Score);
                        if (currentLearnerLog!.Score < maxScore)
                        {
                            currentLearnerLog.Score = maxScore;
                        }

                        var minimumTime = learningObject.LearnerLogs.Min(x => x.TimeTaken);
                        if (currentLearnerLog.TimeTaken > minimumTime)
                        {
                            currentLearnerLog.TimeTaken = minimumTime;
                        }

                        var minimumAttempt = learningObject.LearnerLogs.Min(x => x.Attempt);
                        if (currentLearnerLog.Attempt > minimumAttempt)
                        {
                            currentLearnerLog.Attempt = minimumAttempt;
                        }

                        var currentSimilarity = CalculateSimilarity(learner, learningObject);

                        deltaLearningObjectSimilarity = targetSimilarity - currentSimilarity;

                        if (deltaLearningObjectSimilarity >= 0)
                        {
                            break;
                        }

                        var otherLearnerLogs = learningObject.LearnerLogs.Where(x => x.LearnerId != learnerId).ToList();

                        foreach (var otherLearnerLog in otherLearnerLogs)
                        {
                            if (otherLearnerLog.Score < learningObject.MaxScore)
                            {
                                var newScore = otherLearnerLog.Score + 10;
                                otherLearnerLog.Score = Math.Min(newScore, learningObject.MaxScore);
                            }

                            if (otherLearnerLog.TimeTaken > 1)
                            {
                                var newTime = otherLearnerLog.TimeTaken - 10;
                                otherLearnerLog.TimeTaken = Math.Max(newTime, 1);
                            }
                        }

                        currentSimilarity = CalculateSimilarity(learner, learningObject);

                        deltaLearningObjectSimilarity = targetSimilarity - currentSimilarity;
                    }

                    var currentTopicSimilarity = topic.LearningObjects.Average(lo => CalculateSimilarity(learner, lo));

                    if (currentTopicSimilarity - initialSimilarity <= deltaTopicSimilarity)
                    {
                        break;
                    }
                }
            }
            //var currentCourseSimilarity = CalculateCourseScore(learner, course);
            _unitOfWork.SaveChanges();
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    public async Task<bool> MakeLearnerUseLearningPath(int learnerId)
    {
        var learner = await _unitOfWork.Learners.Find(l => l.Id == learnerId).FirstOrDefaultAsync();

        if (learner == null)
        {
            throw new Exception("Learner not found");
        }

        var learningPathLearningObjectOrders = await _unitOfWork.LearningObjectLearningPathOrders.GetAll()
            .Where(o => o.LearnerId == learnerId)
            .OrderBy(o => o.Order)
            .Include(o => o.LearningObject)
                .ThenInclude(lo => lo.LearnerLogs.Where(ll => ll.LearnerId == learnerId))
            .ToListAsync();

        if (learningPathLearningObjectOrders.Count == 0)
        {
            throw new Exception("Learner has no learning path saved");
        }

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            // chỉnh create at của log theo thứ tự của learning path
            var currentCreatedAt = DateTime.Now;
            foreach (var order in learningPathLearningObjectOrders)
            {
                var currentLearnerLog = order.LearningObject.LearnerLogs.FirstOrDefault(x => x.LearnerId == learnerId);
                currentCreatedAt = currentCreatedAt.AddMinutes(5);
                if (currentLearnerLog == null)
                {
                    currentLearnerLog = new LearnerLog
                    {
                        LearnerId = learnerId,
                        LearningObjectId = order.LearningObjectId,
                        Score = 0,
                        TimeTaken = order.LearningObject.MaxLearningTime,
                        Attempt = 1,
                        CreatedAt = currentCreatedAt
                    };
                    order.LearningObject.LearnerLogs.Add(currentLearnerLog);
                }
                else
                {
                    currentLearnerLog.CreatedAt = currentCreatedAt;
                }
            }

            _unitOfWork.SaveChanges();
            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

    }
    // utility methods
    private const double SCORE_WHEN_NO_LEARNING_OBJECT = double.MaxValue;
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
            .Where(c => c.IsPublished)
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
