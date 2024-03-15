using EducationTech.Annotations;
using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Exceptions.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace EducationTech.Business.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFileService _fileService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
            
        }

        [AllowAnonymous]
        //[Authorize(Policy = "AdminOnly")]
        [HttpGet(Name = "GetWeatherForecast")]
        [Cache(1000)]
        public IEnumerable<WeatherForecast> Get()
        {

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [AllowAnonymous]
        [HttpGet("request")]
        public string GetRequest()
        {
            throw new Exception("something");
            return "cc";
        }

        [AllowAnonymous]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var result = await _fileService.UploadFileAsync(file);
            return Ok(new { result });
        }
    }
}