using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EducationTech.DataAccess.Core;
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
            var globalUsings = new GlobalUsings();
            using (var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "LearnerLogs.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<LearnerLogRecord>();
                var records = csv.GetRecords<LearnerLog>();
                foreach (var record in records)
                {
                    if (_context.LearnerLogs.Any(x =>
                        x.LearnerId == record.LearnerId &&
                        x.LearningObjectId == record.LearningObjectId &&
                        x.Rating == record.Rating &&
                        x.Score == record.Score &&
                        x.Attempt == record.Attempt &&
                        x.TimeTaken == record.TimeTaken &&
                        x.VisitedAt == record.VisitedAt &&
                        x.VisitedTime == record.VisitedTime))
                    {
                        continue;
                    }

                    _context.LearnerLogs.Add(record);
                }
                _context.SaveChanges();
            }
        }
    }

    public class LearnerLogRecord : ClassMap<LearnerLog>
    {
        public LearnerLogRecord()
        {
            Map(x => x.LearnerId).Name("LearnerId").TypeConverter<Int32Converter>();
            Map(x => x.LearningObjectId).Name("LearningObjectId").TypeConverter<Int32Converter>();
            Map(x => x.Rating).Name("Rating").TypeConverter<RatingConverter>();
            Map(x => x.Score).Name("Score").TypeConverter<Int32Converter>();
            Map(x => x.Attempt).Name("Attempts").TypeConverter<Int32Converter>();
            Map(x => x.TimeTaken).Name("TimeTaken").TypeConverter<Int32Converter>();
            //2023-06-11T21:16:58Z
            Map(x => x.VisitedAt).Name("VisitedAt").TypeConverter<DateTimeConverter>();

            Map(x => x.VisitedTime).Name("VisitedTime").TypeConverter<Int32Converter>();
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
