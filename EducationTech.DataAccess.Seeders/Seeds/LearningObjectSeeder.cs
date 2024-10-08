using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
using EducationTech.Storage;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class LearningObjectSeeder : Seeder
    {
        public LearningObjectSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            var globalUsings = GlobalReference.Instance;
            using (var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "LearningObjects.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<LearningObjectRecord>();
                var records = csv.GetRecords<LearningObject>();
                foreach (var record in records)
                {
                    if (_context.LearningObjects.Any(x =>
                        x.Title == record.Title &&
                        x.TopicId == record.TopicId &&
                        x.Structure == record.Structure &&
                        x.AggregationLevel == record.AggregationLevel &&
                        x.Format == record.Format &&
                        x.LearningResourceType == record.LearningResourceType &&
                        x.InteractivityType == record.InteractivityType &&
                        x.InteractivityLevel == record.InteractivityLevel &&
                        x.SemanticDensity == record.SemanticDensity &&
                        x.MaxScore == record.MaxScore &&
                        x.MaxLearningTime == record.MaxLearningTime &&
                        x.Type == record.Type &&
                        x.Difficulty == record.Difficulty
                        ))
                    {
                        continue;
                    }
                    _context.LearningObjects.Add(record);
                }
                _context.SaveChanges();
            }
        }
    }

    public class LearningObjectRecord : ClassMap<LearningObject>
    {
        public LearningObjectRecord()
        {
            Map(x => x.Title).Name("Title");
            Map(x => x.TopicId).Name("TopicId").TypeConverter<Int32Converter>();
            //use enum converter
            Map(x => x.Structure).Name("Structure").TypeConverter(new EnumConverter(typeof(Structure))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.AggregationLevel).Name("AggregationLevel").TypeConverter(new EnumConverter(typeof(AggregationLevel))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.Format).Name("Format").TypeConverter(new EnumConverter(typeof(Format))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.LearningResourceType).Name("LearningResourceType").TypeConverter(new EnumConverter(typeof(LearningResourceType))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.InteractivityType).Name("InteractivityType").TypeConverter(new EnumConverter(typeof(InteractivityType))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.InteractivityLevel).Name("InteractivityLevel").TypeConverter(new EnumConverter(typeof(InteractivityLevel))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.SemanticDensity).Name("SemanticDensity").TypeConverter(new EnumConverter(typeof(SemanticDensity))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.MaxScore).Name("MaxScore").TypeConverter<Int32Converter>();
            Map(x => x.MaxLearningTime).Name("MaxLearningTime").TypeConverter<Int32Converter>();
            Map(x => x.Type).Name("LOexev").TypeConverter(new EnumConverter(typeof(LOType))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.Difficulty).Name("Difficulty").TypeConverter<Int32Converter>();
        }
    }
}
