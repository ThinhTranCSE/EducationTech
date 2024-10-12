using EducationTech.Business.Business.Interfaces;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Http;

namespace EducationTech.Business.Business;

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthService _authService;
    private readonly ICacheService _cacheService;

    public SessionService(IHttpContextAccessor httpContextAccessor, IAuthService authService, ICacheService cacheService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authService = authService;
        _cacheService = cacheService;
    }

    private User? _currentUser;
    public User? CurrentUser
    {
        get
        {
            _currentUser ??= GetCurrentUser().GetAwaiter().GetResult();
            return _currentUser;
        }
    }

    private async Task<User?> GetCurrentUser()
    {
        var hasToken = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
        if (!hasToken)
        {
            return null;
        }

        var user = await _cacheService.GetAsync<User>(token);

        if (user == null)
        {
            user = _authService.GetUserFromToken(token);
            await _cacheService.SetAsync(token, user, TimeSpan.FromMinutes(5));
        }

        return user;
    }
}
