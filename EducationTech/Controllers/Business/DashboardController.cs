using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Dashboards;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Business;

public class DashboardController : BaseController
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<DashboardDto> GetDashboard([FromQuery] IEnumerable<int> specialityIds)
    {
        var dashboard = await _dashboardService.GetDashboard(specialityIds);
        return dashboard;
    }

    [HttpGet("Instructor/{courseId}")]
    public async Task<CourseDashboardDto> GetCourseDashboard(int courseId)
    {
        var courseDashboard = await _dashboardService.GetCourseDashboard(courseId);
        return courseDashboard;
    }
}
