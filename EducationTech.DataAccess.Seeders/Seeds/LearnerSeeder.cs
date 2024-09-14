using CsvHelper;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.Learner;
using EducationTech.Storage;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class LearnerSeeder : Seeder
    {
        public LearnerSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            var globalUsings = new GlobalUsings();
            using (var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "Learners.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                foreach (var record in records)
                {
                    var learner = new Learner
                    {
                        Name = $"{record.first_name} {record.last_name}",
                        Gender = (Gender)Enum.Parse(typeof(Gender), record.gender),
                        Age = Convert.ToInt32(record.age),
                        BackgroundKnowledge = (BackgroundKnowledge)Enum.Parse(typeof(BackgroundKnowledge), record.background_knowledge),
                        Qualification = (Qualification)Enum.Parse(typeof(Qualification), record.qualification),
                        Branch = record.branch
                    };
                    if (_context.Learners.Any(x =>
                        x.Name == learner.Name &&
                        x.Age == learner.Age &&
                        x.Gender == learner.Gender &&
                        x.BackgroundKnowledge == learner.BackgroundKnowledge &&
                        x.Qualification == learner.Qualification
                        ))
                    {
                        continue;
                    }

                    _context.Learners.Add(learner);
                    _context.SaveChanges();
                    learner.LearningStyle = new LearningStyle
                    {
                        LearnerId = learner.Id,
                        Active = Convert.ToSingle(record.active, CultureInfo.InvariantCulture),
                        Reflective = 1 - Convert.ToSingle(record.active, CultureInfo.InvariantCulture),
                        Intuitive = Convert.ToSingle(record.intuitive, CultureInfo.InvariantCulture),
                        Sensing = 1 - Convert.ToSingle(record.intuitive, CultureInfo.InvariantCulture),
                        Visual = Convert.ToSingle(record.visual, CultureInfo.InvariantCulture),
                        Verbal = 1 - Convert.ToSingle(record.visual, CultureInfo.InvariantCulture),
                        Global = Convert.ToSingle(record.global, CultureInfo.InvariantCulture),
                        Sequential = 1 - Convert.ToSingle(record.global, CultureInfo.InvariantCulture),
                    };
                    _context.LearningStyles.Add(learner.LearningStyle);
                    _context.SaveChanges();
                }
            }
        }
    }
}
