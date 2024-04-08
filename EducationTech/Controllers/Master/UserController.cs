using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.User;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger, EducationTechContext context, IAuthService authService) : base(context, authService)
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

        [HttpPatch("{id}")]
        public async Task<User?> UpdateUser(Guid id, [FromBody] User_UpdateDto updateDto)
        {
            return await _userService.UpdateUser(id, updateDto, CurrentUser);
        }
    }
}
