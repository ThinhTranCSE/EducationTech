using Bogus;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class CourseSeeder : Seeder
    {
        public CourseSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            var dataGenerator = new Faker<Course>();
            Random random = new Random();
            int userCount = _context.Users.Count();

            var courses = dataGenerator
                .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Price, f => f.Random.Double(100000, 3000000))
                .RuleFor(x => x.OwnerId, f => _context.Users.Skip(random.Next(0, userCount)).FirstOrDefault()?.Id)
                .RuleFor(x => x.IsArchived, f => f.Random.Bool())
                .RuleFor(x => x.ImageUrl, f => f.Image.PicsumUrl())
                .Generate(50);
        }
    }
}
