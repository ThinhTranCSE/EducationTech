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

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return await ExecuteAsync(async () => await _userService.Get(1));
        }

    }
}
