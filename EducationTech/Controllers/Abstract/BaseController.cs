using EducationTech.Business.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EducationTech.Controllers.Abstract
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        private readonly EducationTechContext _context;
        private readonly IAuthService _authService;

        private User? _currentUser { get; set; }
        public User? CurrentUser
        {
            get
            {
                _currentUser ??= _authService.GetUserFromToken(HttpContext?.Request?.Headers?.Authorization);

                return _currentUser;
            }

            private set { _currentUser = value; }
        }

        public BaseController(EducationTechContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }


        //protected async Task<IActionResult> ExecuteAsync(Func<Task<object?>> executedFunction)
        //{
        //    try
        //    {
        //        var result = await executedFunction();
        //        if (result != null)
        //        {
        //            if (result is bool && !(bool)result)
        //            {
        //                return BadRequest(new ResponseMessage { Status = 400, Message = "Failed" });
        //            }
        //            return Ok(new ResponseMessage { Status = 200, Message = "Success", Data = result });
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseMessage { Status = 400, Message = "Failed" });
        //        }
        //    }
        //    catch (HttpException ex)
        //    {
        //        return StatusCode(ex.StatusCode, new ResponseMessage { Status = ex.StatusCode, Message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ResponseMessage
        //        {
        //            Status = 500,
        //            Message = new
        //            {
        //                ex.Message,
        //                ex.StackTrace
        //            },
        //        });
        //    }
        //}
    }
}
