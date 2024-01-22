using EducationTech.Databases;
using EducationTech.Exceptions.Http;
using EducationTech.Models.Master;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EducationTech.Controllers.Abstract
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        private readonly MainDatabaseContext _context;
        private readonly IAuthUtils _authUtils;

        private User? _currentUser { get; set; }
        public User? CurrentUser
        {
            get
            {
                _currentUser ??= GetUserFromToken(HttpContext?.Request?.Headers?.Authorization);

                return _currentUser;
            }

            private set {  _currentUser = value; }
        }

        public BaseController(MainDatabaseContext context, IAuthUtils authUtils)
        {
            _context = context;
            _authUtils = authUtils;
        }


        protected async Task<IActionResult> ExecuteAsync(Func<Task<object?>> executedFunction)
        {
            try
            {
                var result = await executedFunction();
                if (result != null )
                {
                    if(result is bool && !(bool) result)
                    {
                        return BadRequest(new ResponseMessage { Status = 400, Message = "Failed" });
                    }
                    return Ok(new ResponseMessage { Status = 200, Message = "Success", Data = result});
                }
                else
                {
                    return BadRequest(new ResponseMessage { Status = 400, Message = "Failed" });
                }
            }
            catch (HttpException ex)
            {
                return StatusCode(ex.StatusCode, new ResponseMessage { Status = ex.StatusCode, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage
                {
                    Status = 500,
                    Message = new
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    },
                });
            }
        }

        private User? GetUserFromToken(string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                return null;
            }
            Guid? userId = _authUtils.GetUserIdFromToken(token.Split(" ")[1]);
            return _context.Users.Where(u => u.Id ==  userId).FirstOrDefault();
        }
   
    }
    public class ResponseMessage
    {
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; } = 200;

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public object? Message { get; set; } = null;

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object? Data { get; set; } = null;

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public object? Errors { get; set; } = null;
    }
}
