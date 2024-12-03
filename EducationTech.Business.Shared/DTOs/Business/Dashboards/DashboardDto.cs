namespace EducationTech.Business.Shared.DTOs.Business.Dashboards;

public class DashboardDto
{
    public UserStatistic UserStatistic { get; set; }
    public LearningPathStatistic LearningPathStatistic { get; set; }
}

public class UserStatistic
{
    public int TotalUserCount { get; set; }
    public ICollection<RoleStatistic> RoleStatistics { get; set; } = new List<RoleStatistic>();
}

public class RoleStatistic
{
    public string RoleName { get; set; }
    public int UserCount { get; set; }
}

public class LearningPathStatistic
{
    public int TotalSavedLearningPath { get; set; }
    public int TotalUsedLearningPath { get; set; }
    public ICollection<LearningPathScoreStatistic> LearningPathScoreStatistics { get; set; } = new List<LearningPathScoreStatistic>();
}


public class LearningPathScoreStatistic
{
    public string? SpecialityName { get; set; }
    public ICollection<LearningPathBandScoreStatistic> BandScoreStatistics { get; set; } = new List<LearningPathBandScoreStatistic>();
}

public class LearningPathBandScoreStatistic
{
    public int MinScoreBand { get; set; }
    public int MaxScoreBand { get; set; }
    public int UsedLearningPathCount { get; set; }
    public int NotUsedLearningPathCount { get; set; }
}
