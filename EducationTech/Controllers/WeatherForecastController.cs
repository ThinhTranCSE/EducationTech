using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.RecommendationSystem.DataStructures;
using EducationTech.RecommendationSystem.Implementations.LearnerCollaborativeFilters;
using EducationTech.RecommendationSystem.Implementations.SequenceMiners;
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
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly IRecommendTopicRepository _recommendTopicRepository;
        private readonly ILoSequenceRecommender _loSequenceRecommender;
        public WeatherForecastController(
            IFileService fileService,
            IFileUtils fileUtils,
            ILearnerRepository learnerRepository,
            ILearningObjectRepository learningObjectRepository,
            IRecommendTopicRepository recommendTopicRepository,
            ILoSequenceRecommender loSequenceRecommender
            )
        {
            _fileService = fileService;
            _fileUtils = fileUtils;
            _learnerRepository = learnerRepository;
            _learnerCollaborativeFilter = new CosineSimilarityLearnerFilter();
            _learningObjectRepository = learningObjectRepository;
            _recommendTopicRepository = recommendTopicRepository;
            _loSequenceRecommender = loSequenceRecommender;

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

        //[AllowAnonymous]
        //[HttpGet("TestTopNSimilarityLearners/{id}")]
        //public async Task<IActionResult> TestTopNSimilarityLearners(int id)
        //{

        //    var learner = (await _learnerRepository.Get()).Where(x => x.Id == id).FirstOrDefault();
        //    var similarLearners = await _learnerCollaborativeFilter.TopNSimilarityLearners(learner, 5);

        //    return Ok(similarLearners.AsEnumerable().Select(s => (s.Key.Id, s.Value)));
        //}

        //[AllowAnonymous]
        //[HttpGet("TestRecommendTopNLearningObjects/{learnerId}")]
        //public async Task<IActionResult> TestRecommendTopNLearningObjects(int learnerId)
        //{
        //    var learner = (await _learnerRepository.Get()).Where(x => x.Id == learnerId).Include(x => x.LearningStyle).FirstOrDefault();
        //    var recommender = new OntologyBasedLoRecommender(_learningObjectRepository);
        //    var learningObjects = await recommender.RecommendTopNLearningObjects(learner, 5);

        //    return Ok(learningObjects);
        //}

        [AllowAnonymous]
        [HttpGet("TestPrefixSpanSequenceMiner")]
        public async Task<IActionResult> TestPrefixSpanSequenceMiner()
        {
            var database = DataGenerator.GenerateData(1000, 100, 10);
            var miner = new PrefixSpanSequenceMiner<string>();
            var result = new List<FrequentSequence<string>>();
            miner.MineSequences(new List<List<string>>(), database, 3, result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("TestLoSequenceRecommender")]
        public async Task<IActionResult> TestLoSequenceRecommender(int learnerId, int topicId)
        {
            var learner = (await _learnerRepository.Get(x => x.Id == learnerId)).FirstOrDefault();
            var topic = (await _recommendTopicRepository.Get(x => x.Id == topicId)).FirstOrDefault();
            var sequences = await _loSequenceRecommender.RecommendTopNLearningObjectSequences(learner, topic);

            return Ok(sequences);
        }
    }
}