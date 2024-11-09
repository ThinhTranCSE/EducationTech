using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.RecommendationSystem.Interfaces;
using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly IRecommendTopicRepository _recommendTopicRepository;
        private readonly ILoSequenceRecommender _loSequenceRecommender;
        private readonly ILoSuitableSelector _loSuitableSelector;
        private readonly ILoPathVisitor _loPathVisitor;
        private readonly ILearningPathRecommender _learningPathRecommender;
        public WeatherForecastController(
            IFileService fileService,
            IFileUtils fileUtils,
            ILearnerRepository learnerRepository,
            ILearningObjectRepository learningObjectRepository,
            IRecommendTopicRepository recommendTopicRepository,
            ILearningPathRecommender learningPathRecommender

            )
        {
            _fileService = fileService;
            _fileUtils = fileUtils;
            _learnerRepository = learnerRepository;
            _learningObjectRepository = learningObjectRepository;
            _recommendTopicRepository = recommendTopicRepository;
            _learningPathRecommender = learningPathRecommender;

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
        [HttpGet("TestRecommendLearningPath")]
        public async Task<IActionResult> TestRecommendLearningPath(int learnerId, int startTopicId, int targetTopicId)
        {
            //check time excute 
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var learner = _learnerRepository.Find(x => x.Id == learnerId).FirstOrDefault();
            var startTopic = _recommendTopicRepository
                .Find(x => x.Id == startTopicId)
                .Include(t => t.NextTopicConjuctions)
                .ThenInclude(tc => tc.NextTopic)
                .FirstOrDefault();
            var targetTopic = _recommendTopicRepository
                .Find(x => x.Id == targetTopicId)
                .Include(t => t.NextTopicConjuctions)
                .ThenInclude(tc => tc.NextTopic)
                .FirstOrDefault();

            var learningObjects = await _learningPathRecommender.RecommendLearningPath(learner, startTopic, targetTopic);

            watch.Stop();

            var result = learningObjects.Select(lo => new
            {

                Topic = lo.Topic.Name,
                LO = $"{lo.Title} - {lo.Type}"
            }).ToList();
            return Ok(new
            {
                ExcutedTime = watch.ElapsedMilliseconds,
                Result = result
            });
        }

    }
}