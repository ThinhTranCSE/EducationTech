using EducationTech.Controllers.Abstract;
using EducationTech.DTOs.Business.Auth;
using EducationTech.Services.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Business
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return await ExecuteAsync(async () => await _authService.Login(loginDto));
        }
    }
}
