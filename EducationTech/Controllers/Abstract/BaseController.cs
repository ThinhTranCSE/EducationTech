using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Abstract
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseController : ControllerBase
    {
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
   
        protected class ResponseMessage
        {
            public int Status { get; set; } = 200;
            public object? Message { get; set; } = null;
            public object? Data { get; set; } = null;
        }
    }
}
