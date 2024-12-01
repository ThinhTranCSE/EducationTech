using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Dashboards;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using Neo4jClient.DataAnnotations.Cypher.Functions;

namespace EducationTech.Business.Business;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DashboardDto> GetDashboard(IEnumerable<int> specialityIds)
    {
        var userStatistic = await CalculateUserStatistic();
        var learningPathStatistic = await CalculateLearningPathStatistic(specialityIds);

        return new DashboardDto
        {
            UserStatistic = userStatistic,
            LearningPathStatistic = learningPathStatistic
        };
    }

    private async Task<UserStatistic> CalculateUserStatistic()
    {
        var query = _unitOfWork.UserRoles.GetAll();

        query = query
            .Include(x => x.Role);

        var roleStatistics = await query
            .GroupBy(x => x.Role.Name)
            .Select(x => new RoleStatistic
            {
                RoleName = x.Key,
                UserCount = x.Count()
            })
            .ToListAsync();

        var totalUserCount = _unitOfWork.Users.GetAll().Count();

        return new UserStatistic
        {
            TotalUserCount = totalUserCount,
            RoleStatistics = roleStatistics
        };
    }

    private async Task<LearningPathStatistic> CalculateLearningPathStatistic(IEnumerable<int> specialityIds)
    {
        var query = _unitOfWork.Learners.GetAll();

        query = query
            .Include(l => l.CourseLearningPathOrders)
                .ThenInclude(o => o.Course)
            .Include(l => l.TopicLearningPathOrders)
                .ThenInclude(o => o.Topic)
            .Include(l => l.LearningObjectLearningPathOrders)
                .ThenInclude(o => o.LearningObject)
            .Include(l => l.LearnerLogs);


        if (specialityIds.Any())
        {
            query = query
                .Where(l => specialityIds.Contains(l.SpecialityId));
        }


        var savedLearningPathLearners = await query.ToListAsync();




        var totalSavedLearningPath = savedLearningPathLearners.Count();

        var usedLearningPathLearners = new List<Learner>();

        foreach (var learner in savedLearningPathLearners)
        {
            if (!learner.LearnerLogs.Any())
            {
                continue;
            }

            if (IsLearningPathUsed(learner))
            {
                usedLearningPathLearners.Add(learner);
            }
        }

        var totalUsedLearningPath = usedLearningPathLearners.Count();

        var notUsedLearningPathLearners = savedLearningPathLearners.Except(usedLearningPathLearners).ToList();

        var learningPathScoreStatistics = new List<LearningPathScoreStatistic>();

        var usedLearningPathScores = usedLearningPathLearners
            .Select(l => CalculateLearnerScore(l))
            .ToList();
        var notUsedLearningPathScores = notUsedLearningPathLearners
            .Select(l => CalculateLearnerScore(l))
            .ToList();

        foreach (var specialityId in specialityIds)
        {
            var speciality = await _unitOfWork.Specialities.GetAll().FirstOrDefaultAsync(s => s.Id == specialityId);

            var bandScoreStatistics = new List<LearningPathBandScoreStatistic>();

            for (int i = 0; i < 10; i++)
            {
                var minScoreBand = i * 10;
                var maxScoreBand = (i + 1) * 10;

                var usedLearningPathCount = usedLearningPathScores.Count(s => s >= minScoreBand && s < maxScoreBand);

                var notUsedLearningPathCount = notUsedLearningPathScores.Count(s => s >= minScoreBand && s < maxScoreBand);

                bandScoreStatistics.Add(new LearningPathBandScoreStatistic
                {
                    MinScoreBand = minScoreBand,
                    MaxScoreBand = maxScoreBand,
                    UsedLearningPathCount = usedLearningPathCount,
                    NotUsedLearningPathCount = notUsedLearningPathCount
                });
            }

            learningPathScoreStatistics.Add(new LearningPathScoreStatistic
            {
                SpecialityName = speciality.Name,
                BandScoreStatistics = bandScoreStatistics
            });
        }

        return new LearningPathStatistic
        {
            TotalSavedLearningPath = totalSavedLearningPath,
            TotalUsedLearningPath = totalUsedLearningPath,
            LearningPathScoreStatistics = learningPathScoreStatistics
        };
    }

    private double CalculateLearnerScore(Learner learner)
    {
        var learnerLogs = learner.LearnerLogs
            .ToList();

        if (!learnerLogs.Any())
        {
            return 0;
        }
        double score = 0;

        foreach (var learnerLog in learnerLogs)
        {
            score += learnerLog.Score;
        }

        return score / learnerLogs.Count;
    }

    private bool IsLearningPathUsed(Learner learner)
    {
        var learnerLogs = learner.LearnerLogs
                .OrderBy(ll => ll.CreatedAt)
                .ToList();

        int order = 1;
        var learningObjectLearnedOrderLookup = learnerLogs.ToDictionary(ll => ll.LearningObjectId, ll => order++);

        var semesters = learner.CourseLearningPathOrders
            .GroupBy(o => o.Semester)
            .Select(g => new GroupSemester
            {
                Semester = g.Key,
                Courses = g.Select(g => new GroupCourse
                {
                    CourseId = g.CourseId,
                    Order = g.Order,
                    Topics = learner.TopicLearningPathOrders
                        .Where(t => t.Topic.CourseId == g.CourseId)
                        .Select(t => new GroupTopic
                        {
                            TopicId = t.TopicId,
                            Order = t.Order,
                            LearningObjects = learner.LearningObjectLearningPathOrders
                                .Where(lo => lo.LearningObject.TopicId == t.TopicId)
                                .Select(lo => new GroupLearningObject
                                {
                                    LearningObjectId = lo.LearningObjectId,
                                    Order = lo.Order
                                })
                                .OrderBy(lo => lo.Order)
                                .ToList()
                        })
                        .OrderBy(o => o.Order)
                        .ToList()
                })
                .OrderBy(g => g.Order)
                .ToList()
            })
            .OrderBy(g => g.Semester)
            .ToList();

        bool isUsed = true;

        foreach (var semester in semesters)
        {
            foreach (var course in semester.Courses)
            {
                foreach (var topic in course.Topics)
                {
                    int currentOrder = -1;

                    // kiểm tra học có liền mạch không
                    bool isNotFoundBeforeLog = false;

                    foreach (var learningObject in topic.LearningObjects)
                    {
                        if (learningObjectLearnedOrderLookup.TryGetValue(learningObject.LearningObjectId, out var learnedOrder))
                        {
                            if (isNotFoundBeforeLog)
                            {
                                isUsed = false;
                                break;
                            }
                            if (learnedOrder < currentOrder)
                            {
                                isUsed = false;
                                break;
                            }

                            currentOrder = learnedOrder;
                        }
                        else
                        {
                            isNotFoundBeforeLog = true;
                        }
                    }

                    if (!isUsed)
                    {
                        break;
                    }
                }

                if (!isUsed)
                {
                    break;
                }
            }
        }

        return isUsed;
    }

}

public class GroupSemester
{
    public int Semester { get; set; }
    public List<GroupCourse> Courses { get; set; }
}

public class GroupCourse
{
    public int CourseId { get; set; }
    public int Order { get; set; }
    public List<GroupTopic> Topics { get; set; } = new List<GroupTopic>();
}

public class GroupTopic
{
    public int TopicId { get; set; }
    public int Order { get; set; }
    public List<GroupLearningObject> LearningObjects { get; set; } = new List<GroupLearningObject>();
}

public class GroupLearningObject
{
    public int LearningObjectId { get; set; }
    public int Order { get; set; }
}
