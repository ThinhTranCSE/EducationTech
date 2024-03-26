using EducationTech.Annotations;
using EducationTech.Business.Controllers.Abstract;
using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Services.Master.Interfaces;
using EducationTech.Databases;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Business.Controllers.Master
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger, EducationTechContext context, IAuthUtils authUtils) : base(context, authUtils)
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
