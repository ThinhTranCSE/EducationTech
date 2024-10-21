using CsvHelper;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Storage;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds;

public class CourseGroupSeeder : Seeder
{
    public CourseGroupSeeder(EducationTechContext context) : base(context)
    {
    }

    public override void Seed()
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var globalUsings = GlobalReference.Instance;
            using var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "CourseGroups.csv"));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<CourseGroupRecord>();

            foreach (var record in records)
            {
                if (_context.CourseGroups.Any(x => x.Name == record.Name))
                {
                    continue;
                }
                _context.CourseGroups.Add(new CourseGroup
                {
                    Name = record.Name,
                    MinCredits = record.MinCredits
                });
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

public class CourseGroupRecord
{
    public string Name { get; set; }
    public int MinCredits { get; set; }
}
