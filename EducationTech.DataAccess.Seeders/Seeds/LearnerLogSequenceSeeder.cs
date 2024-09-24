using Bogus;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class LearnerLogSequenceSeeder : Seeder
    {
        private readonly Random _random = new Random();
        private readonly Faker<LearnerLog> _learnerLogFaker;
        public LearnerLogSequenceSeeder(EducationTechContext context) : base(context)
        {
            _learnerLogFaker = new Faker<LearnerLog>()
                .RuleFor(x => x.Rating, f => f.Random.Number(1, 5))
                .RuleFor(x => x.Score, f => f.Random.Number(0, 100))
                .RuleFor(x => x.Attempt, f => f.Random.Number(1, 5))
                .RuleFor(x => x.TimeTaken, f => f.Random.Number(1, 100))
                .RuleFor(x => x.VisitedTime, f => f.Random.Number(1, 100));
        }

        public override void Seed()
        {
            var learners = _context.Learners.ToList();
            var topics = _context.RecommendTopics.Include(x => x.LearningObjects).ToList();

            var learnerLogs = new List<LearnerLog>();

            // Sinh logs cho learners
            foreach (var topic in topics)
            {
                var learningObjects = topic.LearningObjects.ToList();
                var generatedLogs = GenerateLearnerLogs(learners, learningObjects, 3, 10);
                learnerLogs.AddRange(generatedLogs);
            }

            _context.LearnerLogs.AddRange(learnerLogs);
            _context.SaveChanges();
        }

        public List<LearnerLog> GenerateLearnerLogs(List<Learner> learners, List<LearningObject> learningObjects, int minSupport, int minSequences)
        {
            var learnerLogs = new List<LearnerLog>();

            foreach (var learner in learners)
            {
                var learnerLogsForLearner = new List<LearnerLog>();
                var commonSequences = new List<List<int>>(); // Danh sách các chuỗi phổ biến của learner này

                // Xác định thời gian bắt đầu để tạo logs theo thứ tự thời gian
                DateTime currentVisitedAt = DateTime.Now.AddDays(-_random.Next(30, 90)); // Bắt đầu từ một thời điểm cách đây 30-90 ngày

                // Tạo các chuỗi phổ biến cho learner này và đảm bảo min support
                while (commonSequences.Count < minSequences)
                {
                    var sequenceLength = _random.Next(2, 5); // Độ dài sequence ngẫu nhiên
                    var commonSequence = learningObjects
                        .OrderBy(_ => _random.Next())
                        .Take(sequenceLength)
                        .Select(lo => lo.Id)
                        .ToList();

                    commonSequences.Add(commonSequence);

                    // Đảm bảo minSupport cho mỗi sequence
                    for (int i = 0; i < minSupport; i++)
                    {
                        foreach (var learningObjectId in commonSequence)
                        {
                            learnerLogsForLearner.Add(CreateLearnerLog(learner.Id, learningObjectId, ref currentVisitedAt));
                        }
                    }
                }

                // Thêm các logs ngẫu nhiên cho learner với thời gian cập nhật
                var numLogs = _random.Next(20, 50); // Random số lượng log bổ sung cho learner
                for (int i = 0; i < numLogs; i++)
                {
                    var learningObjectId = learningObjects[_random.Next(learningObjects.Count)].Id;
                    learnerLogsForLearner.Add(CreateLearnerLog(learner.Id, learningObjectId, ref currentVisitedAt));
                }

                learnerLogs.AddRange(learnerLogsForLearner);
            }

            return learnerLogs;
        }

        private LearnerLog CreateLearnerLog(int learnerId, int learningObjectId, ref DateTime visitedAt)
        {
            var learnerLog = new LearnerLog
            {
                LearnerId = learnerId,
                LearningObjectId = learningObjectId,
                Rating = _random.Next(1, 6), // Random rating từ 1 đến 5
                Score = _random.Next(0, 101), // Random score từ 0 đến 100
                Attempt = _random.Next(1, 4), // Random số lần thử
                TimeTaken = _random.Next(1, 100), // Random thời gian hoàn thành
                VisitedAt = visitedAt, // Sử dụng thời gian hiện tại đã xác định
                VisitedTime = _random.Next(1, 1000) // Random thời gian ghé thăm (giây)
            };

            // Cập nhật thời gian cho lần ghé thăm tiếp theo
            visitedAt = visitedAt.AddMinutes(_random.Next(30, 120)); // Cộng thêm khoảng thời gian ngẫu nhiên 30-120 phút

            return learnerLog;
        }
    }
}
