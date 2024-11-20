using Bogus;
using CsvHelper;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Shared.Enums;
using EducationTech.Storage;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class CourseSeeder : Seeder
    {
        public override int Piority => 1;
        public CourseSeeder(EducationTechContext context) : base(context)
        {
        }

        public override async void Seed()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS=0;");
                var globalUsings = GlobalReference.Instance;
                using var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "Courses.csv"));
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<CourseRecord>();


                var coursesGenerator = new Faker<Course>();
                Random random = new Random();

                var instructor = _context.Users.FirstOrDefault(x => x.Roles.Any(r => r.Name == nameof(RoleType.Instructor)));
                if (instructor == null)
                {
                    throw new Exception("Please seed the user first.");
                }
                var sampleImage = _context.Images.FirstOrDefault();
                if (sampleImage == null)
                {
                    throw new Exception("No image found, please use api upload at least 1 image first");
                }
                var sampleVideo = _context.Videos.FirstOrDefault();
                if (sampleVideo == null)
                {
                    throw new Exception("No video found, please use api upload at least 1 video first");
                }

                coursesGenerator = coursesGenerator
                    .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                    .RuleFor(x => x.OwnerId, f => instructor.Id)
                    .RuleFor(x => x.IsPublished, f => true)
                    .RuleFor(x => x.PublishedAt, f => f.Date.Past())
                    .RuleFor(x => x.ImageUrl, $"{sampleImage?.Url}");


                var courses = new List<Course>();

                foreach (var record in records)
                {
                    var course = coursesGenerator.Generate();
                    course.Title = record.Name;
                    course.CourseCode = record.CourseId;
                    course.Credits = int.Parse(record.Credits);
                    course.CourseGroupId = !string.IsNullOrEmpty(record.CourseGroupId) ? int.Parse(record.CourseGroupId) : null;
                    course.RecommendedSemester = int.Parse(record.RecommendedSemester);

                    var prerequisites = !string.IsNullOrEmpty(record.Prerequisites) ? record.Prerequisites.Split(", ") : Array.Empty<string>();

                    course.Prerequisites = prerequisites.Select(x => new PrerequisiteCourse
                    {
                        PrerequisiteCourseId = int.Parse(x)
                    }).ToList();

                    var specialityIds = !string.IsNullOrEmpty(record.SpecialityIds) ? record.SpecialityIds.Split(", ") : Array.Empty<string>();

                    course.Specialities = specialityIds.Select(x => new CourseSpeciality
                    {
                        SpecialityId = int.Parse(x)
                    }).ToList();

                    course.Comunity = new Entities.Business.Comunity();

                    courses.Add(course);
                }

                _context.Courses.AddRange(courses);

                _context.SaveChanges();
                transaction.Commit();
                _context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS=1;");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public class CourseRecord
    {
        public string Name { get; set; }
        public string CourseId { get; set; }
        public string Credits { get; set; }
        public string? CourseGroupId { get; set; }
        public string? Prerequisites { get; set; }
        public string RecommendedSemester { get; set; }
        public string? SpecialityIds { get; set; }
        public string BranchIds { get; set; }
    }
}
