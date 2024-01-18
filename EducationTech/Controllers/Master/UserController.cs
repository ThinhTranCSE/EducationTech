using EducationTech.Controllers.Abstract;
using EducationTech.Models.Master;
using EducationTech.Services.Master.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return await ExecuteAsync(async () => await _userService.GetUserById(id));
        }

    }
}
