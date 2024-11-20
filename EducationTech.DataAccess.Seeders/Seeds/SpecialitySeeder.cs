using CsvHelper;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Storage;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds;

public class SpecialitySeeder : Seeder
{
    public SpecialitySeeder(EducationTechContext context) : base(context)
    {
    }

    public override void Seed()
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var globalUsings = GlobalReference.Instance;
            using var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "Specialities.csv"));
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<SpecialityRecord>();

            foreach (var record in records)
            {
                if (_context.Specialities.Any(x => x.Name == record.Name))
                {
                    continue;
                }
                _context.Specialities.Add(new Speciality
                {
                    Name = record.Name,
                    BranchId = record.BranchId
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

public class SpecialityRecord
{
    public string Name { get; set; }
    public int BranchId { get; set; }
}

