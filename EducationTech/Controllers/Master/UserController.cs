using EducationTech.Controllers.Abstract;
using EducationTech.Databases;
using EducationTech.Models.Master;
using EducationTech.Services.Master.Interfaces;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger, MainDatabaseContext context, IAuthUtils authUtils) : base(context, authUtils)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<User?> GetUserById(Guid id)
        {
            return await _userService.GetUserById(id);
        }

        [HttpGet("me")]
        public async Task<User?> GetCurrentUser()
        {
            return CurrentUser;
        }
    }
}
