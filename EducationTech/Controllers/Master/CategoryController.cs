using EducationTech.Business.Business.Interfaces;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master
{
    public class CategoryController : BaseController
    {
        public CategoryController(EducationTechContext context, IAuthService authService) : base(context, authService)
        {
        }

        //[HttpGet]
        //public Task
    }
}
