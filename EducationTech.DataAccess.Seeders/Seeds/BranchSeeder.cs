using CsvHelper;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Storage;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds;

public class BranchSeeder : Seeder
{
    public BranchSeeder(EducationTechContext context) : base(context)
    {
    }

    public override void Seed()
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var globalUsings = GlobalReference.Instance;
            using var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "Branches.csv"));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<BranchRecord>();

            foreach (var record in records)
            {
                if (_context.Branches.Any(x => x.Name == record.Name))
                {
                    continue;
                }
                _context.Branches.Add(new Branch
                {
                    Name = record.Name
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

public class BranchRecord
{
    public string Name { get; set; }
}
