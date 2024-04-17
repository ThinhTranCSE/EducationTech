using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeDetective;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace EducationTech.Controllers
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

        private readonly IFileService _fileService;
        private readonly IFileUtils _fileUtils;

        public WeatherForecastController(IFileService fileService, IFileUtils fileUtils)
        {
            _fileService = fileService;
            _fileUtils = fileUtils;
        }

        [AllowAnonymous]
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
        [HttpGet("test-inspect")]
        public async Task<IActionResult> TestInspect()
        {
            string filePath = @"C:\Users\thinh\source\repos\EducationTech\EducationTech.Storage\Static\cut_100_first_bytes.py";
            var result = await _fileUtils.InspectContent(filePath);
            return Ok(new { result });
        }
    }
}