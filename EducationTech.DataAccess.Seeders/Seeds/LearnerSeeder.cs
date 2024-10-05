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
            var globalUsings = GlobalReference.Instance;
            using (var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "Learners.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                foreach (var record in records)
                {
                    var learner = new Learner
                    {
                        Name = record.Name,
                        Gender = (Gender)Enum.Parse(typeof(Gender), record.Gender),
                        Age = Convert.ToInt32(record.Age),
                        BackgroundKnowledge = (BackgroundKnowledge)Enum.Parse(typeof(BackgroundKnowledge), record.BackgroundKnowledge),
                        Qualification = (Qualification)Enum.Parse(typeof(Qualification), record.Qualification),
                        Branch = record.Branch
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
                        Active = Convert.ToSingle(record.Active, CultureInfo.InvariantCulture),
                        Reflective = 1 - Convert.ToSingle(record.Active, CultureInfo.InvariantCulture),
                        Intuitive = Convert.ToSingle(record.Intuitive, CultureInfo.InvariantCulture),
                        Sensing = 1 - Convert.ToSingle(record.Intuitive, CultureInfo.InvariantCulture),
                        Visual = Convert.ToSingle(record.Visual, CultureInfo.InvariantCulture),
                        Verbal = 1 - Convert.ToSingle(record.Visual, CultureInfo.InvariantCulture),
                        Global = Convert.ToSingle(record.Global, CultureInfo.InvariantCulture),
                        Sequential = 1 - Convert.ToSingle(record.Global, CultureInfo.InvariantCulture),
                    };
                    _context.LearningStyles.Add(learner.LearningStyle);
                    _context.SaveChanges();
                }
            }
        }
    }
}
