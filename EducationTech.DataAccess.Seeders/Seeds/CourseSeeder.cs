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
        public override int Piority => 1;
        public CourseSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            var coursesGenerator = new Faker<Course>();
            Random random = new Random();
            int maxUserCount = 10;
            var users = _context.Users.AsQueryable()
                .Take(maxUserCount)
                .ToList();
            var userCount = users.Count;
            if(userCount == 0)
            {
                throw new Exception("No user found in the database. Please seed the user first.");
            }
            var image = _context.Images.FirstOrDefault();
            if(image == null)
            {
                throw new Exception("No image found, please use api upload at least 1 image first");
            }

            var courses = coursesGenerator
                .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Price, f => f.Random.Double(100000, 3000000))
                .RuleFor(x => x.OwnerId, f => users.Skip(random.Next(0, userCount - 1)).FirstOrDefault()?.Id)
                .RuleFor(x => x.IsArchived, f => f.Random.Bool())
                .RuleFor(x => x.ImageUrl, $"{image?.Url}")
                .Generate(50);

            _context.Courses.AddRange(courses);
            _context.SaveChanges();
            var courseSectionsGenerator = new Faker<CourseSection>()
                .RuleFor(x => x.Title, f => f.Lorem.Sentence());
            IEnumerable<CourseSection> courseSections = courses.Select(course =>
            {
                var sectionCount = random.Next(1, 10);
                var courseSections = courseSectionsGenerator.Generate(sectionCount);
                for (int i = 0; i < sectionCount; i++)
                {
                    courseSections[i].CourseId = course.Id;
                    courseSections[i].Order = i;
                }
                return courseSections.AsEnumerable();
            })
            .Aggregate((acc, x) => acc.Concat(x));

            _context.CourseSections.AddRange(courseSections);
            _context.SaveChanges();
        }
    }
}
