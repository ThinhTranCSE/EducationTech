using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Business.Dashboards;

namespace EducationTech.Business.Business.Interfaces;

public interface IDashboardService : IService
{
    Task<DashboardDto> GetDashboard(IEnumerable<int> specialityIds);
    Task<CourseDashboardDto> GetCourseDashboard(int courseId);
}
