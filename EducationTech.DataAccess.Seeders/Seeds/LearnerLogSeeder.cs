using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Storage;
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
                var globalReference = GlobalReference.Instance;
                using var reader = new StreamReader(Path.Combine(globalReference.StaticFilesPath, "LearnerLogs.csv"));
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Context.RegisterClassMap<LearnerLogRecord>();
                var records = csv.GetRecords<LearnerLog>();
                foreach (var record in records)
                {
                    if (_context.LearnerLogs.Any(x =>
                        x.LearnerId == record.LearnerId &&
                        x.LearningObjectId == record.LearningObjectId &&
                        x.Score == record.Score &&
                        x.Attempt == record.Attempt &&
                        x.TimeTaken == record.TimeTaken
                        ))
                    {
                        continue;
                    }

                    _context.LearnerLogs.Add(record);
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
