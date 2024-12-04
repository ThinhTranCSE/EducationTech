using AutoMapper;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Dashboards;
using EducationTech.Business.Shared.DTOs.Masters.Courses;
using EducationTech.Business.Shared.DTOs.Recommendation.LearningPaths;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;

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

        var userRoles = await query.ToListAsync();


        var roleStatistics = userRoles
            .GroupBy(x => x.Role.Name)
            .Select(x => new RoleStatistic
            {
                RoleName = x.Key,
                UserCount = x.Count()
            })
            .ToList();

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
            .Include(l => l.LearnerLogs)
            .Where(l => l.CourseLearningPathOrders.Any());


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

            if (await IsLearningPathUsed(learner))
            {
                usedLearningPathLearners.Add(learner);
            }
        }

        var totalUsedLearningPath = usedLearningPathLearners.Count();

        var notUsedLearningPathLearners = savedLearningPathLearners.Except(usedLearningPathLearners).ToList();

        var learningPathScoreStatistics = new List<LearningPathScoreStatistic>();

        if (specialityIds.Any())
        {
            foreach (var specialityId in specialityIds)
            {
                var speciality = await _unitOfWork.Specialities.GetAll()
                    .Include(s => s.Learners)
                    .FirstOrDefaultAsync(s => s.Id == specialityId);

                var usedLearningPathLearnersForSpeciality = usedLearningPathLearners
                    .Where(l => l.SpecialityId == specialityId)
                    .ToList();

                var notUsedLearningPathLearnersForSpeciality = notUsedLearningPathLearners
                    .Where(l => l.SpecialityId == specialityId)
                    .ToList();

                CalculateLearningPathBandScoreStatistics(learningPathScoreStatistics, speciality, usedLearningPathLearnersForSpeciality, notUsedLearningPathLearnersForSpeciality);
            }
        }
        else
        {
            CalculateLearningPathBandScoreStatistics(learningPathScoreStatistics, null, usedLearningPathLearners, notUsedLearningPathLearners);
        }

        return new LearningPathStatistic
        {
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

    private async Task<bool> IsLearningPathUsed(Learner learner)
    {
        var learnerLogs = learner.LearnerLogs
            .OrderBy(l => l.CreatedAt)
            .ToList();

        var learningObjectIds = learnerLogs.Select(ll => ll.LearningObjectId).ToList();

        int order = 1;
        var learningObjectLearnedOrderLookup = learnerLogs.ToDictionary(ll => ll.LearningObjectId, ll => order++);

        // build cây learning path
        var learnerId = learner.Id;

        var query = _unitOfWork.Courses.GetAll()
            .Include(c => c.CourseLearningPathOrders.Where(o => o.LearnerId == learnerId))
            .Include(c => c.Topics)
                .ThenInclude(t => t.TopicLearningPathOrders.Where(o => o.LearnerId == learnerId))
            .Include(c => c.Topics
                .Where(t => t.LearningObjects.Any(lo => learningObjectIds.Contains(lo.Id))))
                .ThenInclude(t => t.LearningObjects
                    .Where(lo => learningObjectIds.Contains(lo.Id)))
                    .ThenInclude(lo => lo.LearningObjectLearningPathOrders.Where(o => o.LearnerId == learnerId))
            .Where(c => c.CourseLearningPathOrders.Any(o => o.LearnerId == learnerId) &&
                        c.Topics.Any(t => t.LearningObjects.Any(lo => learningObjectIds.Contains(lo.Id))));


        var courses = await query.ToListAsync();

        var semesters = courses.GroupBy(c => c.CourseLearningPathOrders.First().Semester)
            .Select(g => new SemesterCourseDto
            {
                Semester = g.Key,
                Courses = _mapper.ProjectTo<Course_MinimalDto>(g.AsQueryable()).OrderBy(c => c.CourseLearningPathOrders.First().Order).ToList(),
                TotalCredits = 0
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


        var isLearningPathUsed = true;

        foreach (var semester in semesters)
        {
            foreach (var course in semester.Courses)
            {
                foreach (var topic in course.Topics)
                {
                    int lastLearnedOrder = -1;
                    foreach (var learningObject in topic.LearningObjects)
                    {
                        if (!learningObjectLearnedOrderLookup.TryGetValue(learningObject.Id, out var learnedOrder))
                        {
                            isLearningPathUsed = false;
                            break;
                        }

                        if (learningObject.LearningObjectLearningPathOrders.First().Order <= learnedOrder)
                        {
                            isLearningPathUsed = false;
                            break;
                        }

                        lastLearnedOrder = learnedOrder;
                    }

                    if (!isLearningPathUsed)
                    {
                        break;
                    }
                }

                if (!isLearningPathUsed)
                {
                    break;
                }
            }

            if (!isLearningPathUsed)
            {
                break;
            }
        }

        return isLearningPathUsed;
    }

    private void CalculateLearningPathBandScoreStatistics(List<LearningPathScoreStatistic> learningPathScoreStatistics, Speciality? speciality, List<Learner> usedLearningPathLearners, List<Learner> notUsedLearningPathLearners)
    {
        var usedLearningPathScores = usedLearningPathLearners
            .Select(l => CalculateLearnerScore(l))
            .ToList();
        var notUsedLearningPathScores = notUsedLearningPathLearners
            .Select(l => CalculateLearnerScore(l))
            .ToList();

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
            SpecialityName = speciality?.Name,
            TotalLearnerInSpeciality = speciality == null ? speciality!.Learners.Count : 0,
            TotalSavedLearningPath = usedLearningPathLearners.Count + notUsedLearningPathLearners.Count,
            TotalUsedLearningPath = usedLearningPathLearners.Count,
            BandScoreStatistics = bandScoreStatistics
        });

    }
}


