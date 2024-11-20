using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Recommendation;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class LearnerLogSeeder : Seeder
    {
        public LearnerLogSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var learners = _context.Learners.ToList();

                var learningObjects = _context.LearningObjects.ToList();

                var ramdom = new Random();
                foreach (var learningObject in learningObjects)
                {
                    //take 5 random learners
                    var randomLearners = learners.OrderBy(x => Guid.NewGuid()).Take(5).ToList();

                    foreach (var learner in randomLearners)
                    {
                        var learnerLog = new LearnerLog
                        {
                            LearnerId = learner.Id,
                            LearningObjectId = learningObject.Id,
                            Score = ramdom.Next(0, learningObject.MaxScore),
                            Attempt = ramdom.Next(1, 5),
                            TimeTaken = ramdom.Next(10, learningObject.MaxLearningTime)
                        };

                        _context.LearnerLogs.Add(learnerLog);
                    }

                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }
    }

    public class LearnerLogRecord : ClassMap<LearnerLog>
    {
        public LearnerLogRecord()
        {
            Map(x => x.LearnerId).Name("LearnerId").TypeConverter<Int32Converter>();
            Map(x => x.LearningObjectId).Name("LearningObjectId").TypeConverter<Int32Converter>();
            Map(x => x.Score).Name("Score").TypeConverter<Int32Converter>();
            Map(x => x.Attempt).Name("Attempts").TypeConverter<Int32Converter>();
            Map(x => x.TimeTaken).Name("TimeTaken").TypeConverter<Int32Converter>();
        }
    }

    public class RatingConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return (int)float.Parse(text, CultureInfo.InvariantCulture);
        }
    }
}
