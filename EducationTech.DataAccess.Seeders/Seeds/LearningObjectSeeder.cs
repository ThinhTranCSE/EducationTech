using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EducationTech.DataAccess.Core;
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
            var globalUsings = new GlobalUsings();
            using (var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "LearningMaterials.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<LearningObjectRecord>();
                var records = csv.GetRecords<LearningObject>();
                foreach (var record in records)
                {
                    if (_context.LearningObjects.Any(x =>
                        x.Title == record.Title &&
                        x.Structure == record.Structure &&
                        x.AggregationLevel == record.AggregationLevel &&
                        x.Format == record.Format &&
                        x.LearningResourceType == record.LearningResourceType &&
                        x.InteractivityType == record.InteractivityType &&
                        x.InteractivityLevel == record.InteractivityLevel &&
                        x.SemanticDensity == record.SemanticDensity))
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
            Map(x => x.Title).Name("title");
            //use enum converter
            Map(x => x.Structure).Name("structure").TypeConverter(new EnumConverter(typeof(Structure))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.AggregationLevel).Name("aggregation_level").TypeConverter(new EnumConverter(typeof(AggregationLevel))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.Format).Name("format").TypeConverter(new EnumConverter(typeof(Format))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.LearningResourceType).Name("learning_resource_type").TypeConverter(new EnumConverter(typeof(LearningResourceType))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.InteractivityType).Name("interactivity_type").TypeConverter(new EnumConverter(typeof(InteractivityType))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.InteractivityLevel).Name("interactivity_level").TypeConverter(new EnumConverter(typeof(InteractivityLevel))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.SemanticDensity).Name("semantic_density").TypeConverter(new EnumConverter(typeof(SemanticDensity))).TypeConverterOption.EnumIgnoreCase(true);
        }
    }
}
