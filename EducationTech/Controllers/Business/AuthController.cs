using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Auth;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace EducationTech.Controllers.Business
{
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger, EducationTechContext context) : base(context, authService)
        {
            _authService = authService;
            _logger = logger;
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
            return await _authService.RefreshExpiredTokens(CurrentUser.Id);
        }

        [HttpDelete("Logout")]
        public async Task<bool?> Logout()
        {
            return await _authService.Logout(CurrentUser.Id);

        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<User?> Register(RegisterDto registerDto)
        {
            return await _authService.Register(registerDto);
        }

    }
}
