using EducationTech.Controllers.Abstract;
using EducationTech.Models.Master;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    public class UserController : BaseController
    {
        //private readonly IUserService _userService;
        public UserController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string text)
        {
            bool check = true;
            return await ExecuteAsync(async () => await DoSomeWork(check));
        }

        private async Task<int> DoSomeWork(bool param)
        {
            return 1;
        }
    }
}
