using EducationTech.Controllers.Abstract;
using EducationTech.Models.Master;
using EducationTech.Services.Master.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return await ExecuteAsync(async () => await _userService.GetUserById(id));
        }

    }
}
