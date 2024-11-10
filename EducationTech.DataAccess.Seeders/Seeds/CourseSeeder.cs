using Bogus;
using CsvHelper;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Business;
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
                int maxUserCount = 10;
                var users = _context.Users.AsQueryable()
                    .Take(maxUserCount)
                    .ToList();
                var userCount = users.Count;
                if (userCount == 0)
                {
                    throw new Exception("No user found in the database. Please seed the user first.");
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
                    //.RuleFor(x => x.Price, f => f.Random.Double(100000, 3000000))
                    .RuleFor(x => x.OwnerId, f => users.Skip(random.Next(0, userCount - 1)).FirstOrDefault()?.Id)
                    .RuleFor(x => x.IsArchived, f => false)
                    .RuleFor(x => x.IsPublished, f => true)
                    .RuleFor(x => x.PublishedAt, f => f.Date.Past())
                    .RuleFor(x => x.ImageUrl, $"{sampleImage?.Url}");

                var computerSiecne = _context.Categories.FirstOrDefault(x => x.Name == "Computer Science");

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

                    course.CourseCategories = new List<CourseCategory>
                    {
                        new CourseCategory
                        {
                            CategoryId = computerSiecne!.Id
                        }
                    };
                    courses.Add(course);
                }

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

                var lessonGenerator = new Faker<Lesson>()
                    .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                    .RuleFor(x => x.Type, f => f.PickRandom<LessonType>());


                IEnumerable<Lesson> lessons = courseSections.Select(section =>
                {
                    var lessonCount = random.Next(1, 5);
                    var lessons = lessonGenerator.Generate(lessonCount);
                    for (int i = 0; i < lessonCount; i++)
                    {
                        lessons[i].CourseSectionId = section.Id;
                        lessons[i].Order = i;
                    }
                    return lessons.AsEnumerable();
                })
                .Aggregate((acc, x) => acc.Concat(x));

                _context.Lessons.AddRange(lessons);
                _context.SaveChanges();

                IEnumerable<Video> videos = new List<Video>();
                IEnumerable<Quiz> quizzes = new List<Quiz>();

                foreach (var lesson in lessons)
                {
                    if (lesson.Type == LessonType.Video)
                    {
                        var video = new Video
                        {
                            LessonId = lesson.Id,
                            FileId = sampleVideo.FileId,
                            Url = sampleVideo.Url
                        };
                        videos = videos.Append(video);
                    }
                    else if (lesson.Type == LessonType.Quiz)
                    {
                        var quiz = new Quiz
                        {
                            LessonId = lesson.Id,
                            TimeLimit = random.Next(60, 60 * 60)
                        };
                        quizzes = quizzes.Append(quiz);
                    }
                }

                _context.Videos.AddRange(videos);
                _context.Quizzes.AddRange(quizzes);
                _context.SaveChanges();

                var questionsGenerator = new Faker<Question>()
                    .RuleFor(x => x.Content, f => f.Lorem.Sentence());
                IEnumerable<Question> questions = quizzes.Select(quiz =>
                {
                    var questionCount = random.Next(5, 10);
                    var questions = questionsGenerator.Generate(questionCount);
                    for (int i = 0; i < questionCount; i++)
                    {
                        questions[i].QuizId = quiz.Id;
                    }
                    return questions.AsEnumerable();
                })
                .Aggregate((acc, x) => acc.Concat(x));

                _context.Questions.AddRange(questions);
                _context.SaveChanges();

                var answersGenerator = new Faker<Answer>()
                    .RuleFor(x => x.Content, f => f.Lorem.Sentence())
                    .RuleFor(x => x.IsCorrect, f => f.Random.Bool());

                IEnumerable<Answer> answers = questions.Select(question =>
                {
                    var answerCount = random.Next(4, 6);
                    var answers = answersGenerator.Generate(answerCount);
                    for (int i = 0; i < answerCount; i++)
                    {
                        answers[i].QuestionId = question.Id;
                    }
                    return answers.AsEnumerable();
                })
                .Aggregate((acc, x) => acc.Concat(x));
                _context.Answers.AddRange(answers);
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
