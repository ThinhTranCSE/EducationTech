using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Auth;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Business
{
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ISessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<TokensReponseDto> Login(LoginDto loginDto)
        {
            return await _authService.Login(loginDto);
        }

        [Authorize(Policy = "RefreshExpiredToken")]
        [HttpPost("Refresh")]
        public async Task<TokensReponseDto> Refresh()
        {
            return await _authService.RefreshExpiredTokens(_sessionService.CurrentUser!.Id);
        }

        [HttpDelete("Logout")]
        public async Task<bool?> Logout()
        {
            return await _authService.Logout(_sessionService.CurrentUser!.Id);

        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<User?> Register(RegisterDto registerDto)
        {
            return await _authService.Register(registerDto);
        }

    }
}
