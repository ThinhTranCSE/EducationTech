using EducationTech.Business.Controllers.Abstract;
using EducationTech.Business.DTOs.Business.Auth;
using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Databases;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Business.Controllers.Business
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger, MainDatabaseContext context, IAuthUtils authUtils) : base(context, authUtils)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<TokensReponseDto> Login(LoginDto loginDto)
        {
            return await _authService.Login(loginDto);
        }


    }
}
