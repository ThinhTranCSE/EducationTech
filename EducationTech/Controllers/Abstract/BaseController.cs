using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Abstract;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseController : ControllerBase
{

}