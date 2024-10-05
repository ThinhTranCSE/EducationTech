using EducationTech.Annotations;
using EducationTech.Business.Business.Interfaces;
using EducationTech.DataAccess.Recommendation.Interfaces;
using EducationTech.RecommendationSystem.Implementations.LearnerCollaborativeFilters;
using EducationTech.RecommendationSystem.Implementations.LoRecommenders;
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
            ILoSequenceRecommender loSequenceRecommender,
            ILoSuitableSelector loSuitableSelector,
            ILoPathVisitor loPathVisitor,
            ILearningPathRecommender learningPathRecommender

            )
        {
            _fileService = fileService;
            _fileUtils = fileUtils;
            _learnerRepository = learnerRepository;
            _learnerCollaborativeFilter = new CosineSimilarityLearnerFilter();
            _learningObjectRepository = learningObjectRepository;
            _recommendTopicRepository = recommendTopicRepository;
            _loSequenceRecommender = loSequenceRecommender;
            _loSuitableSelector = loSuitableSelector;
            _loPathVisitor = loPathVisitor;
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
        [HttpGet("TestTopNSimilarityLearners/{id}")]
        public async Task<IActionResult> TestTopNSimilarityLearners(int id)
        {

            var learner = (await _learnerRepository.Get()).Where(x => x.Id == id).Include(x => x.LearningStyle).FirstOrDefault();
            var interestedLearners = (await _learnerRepository.Get()).Where(x => x.Id != learner.Id).Include(x => x.LearningStyle).Include(x => x.LearnerLogs).ToList();
            var similarLearners = await _learnerCollaborativeFilter.TopNSimilarityLearners(learner, interestedLearners, 5);

            return Ok(similarLearners.AsEnumerable().Select(s => (s.Key.Id, s.Value)));
        }

        [AllowAnonymous]
        [HttpGet("TestRecommendTopNLearningObjects/{learnerId}")]
        public async Task<IActionResult> TestRecommendTopNLearningObjects(int learnerId)
        {
            var learner = (await _learnerRepository.Get()).Where(x => x.Id == learnerId).Include(x => x.LearningStyle).FirstOrDefault();
            var recommender = new SimilarUserRatingLoRecommender(_learnerCollaborativeFilter);

            var interestedLearners = (await _learnerRepository.Get()).Where(x => x.Id != learnerId).Include(x => x.LearningStyle).Include(x => x.LearnerLogs).ToList();
            var interestedLearningObjects = (await _learningObjectRepository.Get()).Include(x => x.LearnerLogs).ThenInclude(x => x.Learner).ToList();

            var learningObjects = await recommender.RecommendTopNLearningObjects(learner, interestedLearners, interestedLearningObjects, 5);

            return Ok(learningObjects.Select(l => l.Id));
        }

        //[AllowAnonymous]
        //[HttpGet("TestPrefixSpanSequenceMiner")]
        //public async Task<IActionResult> TestPrefixSpanSequenceMiner()
        //{
        //    //db = [
        //    //    [0, 1, 2, 3, 4],
        //    //    [1, 1, 1, 3, 4],
        //    //    [2, 1, 2, 2, 0],
        //    //    [1, 1, 1, 2, 2],
        //    //]

        //    var database = new List<Sequence<string>>
        //    {
        //        new Sequence<string>(new List<List<string>>
        //        {
        //            new List<string> { "0" },
        //            new List<string> { "1" },
        //            new List<string> { "2" },
        //            new List<string> { "3" },
        //            new List<string> { "4" },
        //        }),
        //        new Sequence<string>(new List<List<string>>
        //        {
        //            new List<string> { "1" },
        //            new List<string> { "1" },
        //            new List<string> { "1" },
        //            new List<string> { "3" },
        //            new List<string> { "4" },
        //        }),
        //        new Sequence<string>(new List<List<string>>
        //        {
        //            new List<string> { "2" },
        //            new List<string> { "1" },
        //            new List<string> { "2" },
        //            new List<string> { "2" },
        //            new List<string> { "0" },
        //        }),
        //        new Sequence<string>(new List<List<string>>
        //        {
        //            new List<string> { "1" },
        //            new List<string> { "1" },
        //            new List<string> { "1" },
        //            new List<string> { "2" },
        //            new List<string> { "2" },
        //        }),
        //    };

        //    var miner = new PrefixSpanSequenceMiner<string>();
        //    var result = new List<FrequentSequence<string>>();
        //    miner.MineSequences(new List<List<string>>(), database, 2, result);

        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpGet("TestLoSequenceRecommender")]
        public async Task<IActionResult> TestLoSequenceRecommender(int learnerId, int topicId)
        {
            var learner = (await _learnerRepository.Get(x => x.Id == learnerId)).FirstOrDefault();
            var topic = (await _recommendTopicRepository.Get(x => x.Id == topicId)).FirstOrDefault();
            var sequences = await _loSequenceRecommender.RecommendTopNLearningObjectSequences(learner, topic);

            return Ok(sequences);
        }

        //[AllowAnonymous]
        //[HttpGet("TestSelectSuitableLoPair")]
        //public async Task<IActionResult> TestSelectSuitableLoPair(int learnerId, int topicId)
        //{
        //    var learner = (await _learnerRepository.Get(x => x.Id == learnerId)).FirstOrDefault();
        //    var topic = (await _recommendTopicRepository.Get(x => x.Id == topicId)).Include(x => x.NextTopicConjuctions).ThenInclude(x => x.NextTopic).FirstOrDefault();
        //    var (exLo, evLo) = await _loSuitableSelector.SelectSuitableLoPair(learner, topic);

        //    return Ok(new List<int> { exLo.Id, evLo.Id });
        //}

        //[AllowAnonymous]
        //[HttpGet("TestSelectAllLoPaths")]
        //public async Task<IActionResult> TestSelectAllLoPaths(int learnerId, int startTopicId, int targetTopicId)
        //{
        //    var learner = (await _learnerRepository.Get(x => x.Id == learnerId)).FirstOrDefault();
        //    var startTopic = (await _recommendTopicRepository.Get(x => x.Id == startTopicId)).Include(t => t.NextTopicConjuctions).ThenInclude(tc => tc.NextTopic).FirstOrDefault();
        //    var targetTopic = (await _recommendTopicRepository.Get(x => x.Id == targetTopicId)).Include(t => t.NextTopicConjuctions).ThenInclude(tc => tc.NextTopic).FirstOrDefault();
        //    var paths = await _loPathVisitor.SelectAllLoPaths(learner, startTopic, targetTopic);

        //    return Ok(paths);
        //}

        [AllowAnonymous]
        [HttpGet("TestRecommendLearningPath")]
        public async Task<IActionResult> TestRecommendLearningPath(int learnerId, int startTopicId, int targetTopicId)
        {
            var learner = (await _learnerRepository.Get(x => x.Id == learnerId)).FirstOrDefault();
            var startTopic = (await _recommendTopicRepository.Get(x => x.Id == startTopicId)).Include(t => t.NextTopicConjuctions).ThenInclude(tc => tc.NextTopic).FirstOrDefault();
            var targetTopic = (await _recommendTopicRepository.Get(x => x.Id == targetTopicId)).Include(t => t.NextTopicConjuctions).ThenInclude(tc => tc.NextTopic).FirstOrDefault();
            var learningObjects = await _learningPathRecommender.RecommendLearningPath(learner, startTopic, targetTopic);

            var learningObjectIds = learningObjects.Select(lo => lo.Id).ToList();
            return Ok(learningObjectIds);
        }

    }
}