using EducationTech.Business.Controllers.Abstract;
using EducationTech.Business.DTOs.Business.Auth;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Databases;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace EducationTech.Business.Controllers.Business
{
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger, MainDatabaseContext context, IAuthUtils authUtils) : base(context, authUtils)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<TokensReponseDto> Login(LoginDto loginDto)
        {
            return await _authService.Login(loginDto);
        }

        [Authorize(Policy = "RefreshExpiredToken")]
        [HttpPost("refresh")]
        public async Task<TokensReponseDto> Refresh()
        {
            return await _authService.RefreshExpiredTokens(CurrentUser.Id);
        }

        [HttpDelete("logout")]
        public async Task<bool?> Logout()
        {
            return await _authService.Logout(CurrentUser.Id);

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<User?> Register(RegisterDto registerDto)
        {
            return await _authService.Register(registerDto);
        }

    }
}
