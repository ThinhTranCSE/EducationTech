using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master.Interfaces
{
    public interface IUserController
    {
        Task<IActionResult> GetAllUsers();
    }
}
