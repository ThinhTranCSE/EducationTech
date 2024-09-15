using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.RecommendationSystem.Implementations.LearnerCollaborativeFilters;
using EducationTech.RecommendationSystem.Interfaces;
using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        private readonly ILearnerCollaborativeFilter _learnerCollaborativeFilter;
        private readonly ILearnerRepository _learnerRepository;

        public WeatherForecastController(IFileService fileService, IFileUtils fileUtils, ILearnerRepository learnerRepository)
        {
            _fileService = fileService;
            _fileUtils = fileUtils;
            _learnerRepository = learnerRepository;
            _learnerCollaborativeFilter = new CosineSimilarityLearnerFilter(_learnerRepository);
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
        [HttpGet("TestTopNSimilarityLearners/{id}")]
        public async Task<IActionResult> TestTopNSimilarityLearners(int id)
        {

            var learner = (await _learnerRepository.Get()).Where(x => x.Id == id).FirstOrDefault();
            var similarLearners = await _learnerCollaborativeFilter.TopNSimilarityLearners(learner, 5);

            return Ok(similarLearners.AsEnumerable().Select(s => (s.Key.Id, s.Value)));
        }
    }
}