using Bogus;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class LearningObjectSeeder : Seeder
    {
        public LearningObjectSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var video = _context.Videos
                    .Include(v => v.File)
                    .FirstOrDefault();

                if (video == null)
                {
                    throw new Exception("No video found, please use api upload at least 1 video first");
                }


                var learningObjectGenerator = new Faker<LearningObject>()
                    .RuleFor(lo => lo.Difficulty, f => 5)
                    .RuleFor(lo => lo.MaxScore, f => 100)
                    .RuleFor(lo => lo.MaxLearningTime, f => 300)
                    .RuleFor(lo => lo.Type, f => f.PickRandom(LOType.Explanatory, LOType.Evaluative))
                    .RuleFor(lo => lo.Title, f => f.Random.AlphaNumeric(5));

                var questionGenerator = new Faker<Question>()
                    .RuleFor(q => q.Content, f => f.Lorem.Sentence());


                var topics = _context.RecommendTopics.ToList();

                foreach (var topic in topics)
                {
                    var learningObjects = learningObjectGenerator.Generate(5);
                    int order = 1;
                    learningObjects.ForEach(lo =>
                    {
                        lo.TopicId = topic.Id;
                        lo.Order = order++;
                        string prefix = lo.Type == LOType.Explanatory ? "Video" : "Quiz";
                        lo.Title = $"{prefix}_{lo.Title}_{lo.Order}";

                        if (lo.Type == LOType.Explanatory)
                        {
                            lo.Video = new Video
                            {
                                Url = video.Url,
                                File = new UploadedFile
                                {
                                    OriginalFileName = video.File.OriginalFileName,
                                    Extension = video.File.Extension,
                                    Size = video.File.Size,
                                    Path = video.File.Path,
                                    IsCompleted = video.File.IsCompleted,
                                    IsPublic = video.File.IsPublic,
                                    FileType = video.File.FileType,
                                    UserId = video.File.UserId
                                }
                            };
                        }
                        else
                        {
                            var questions = questionGenerator.Generate(lo.MaxScore / 10);

                            foreach (var question in questions)
                            {
                                question.Answers = new List<Answer>
                                   {
                                    new Answer
                                    {
                                        Content = "Đây là đáp án đúng (phục vụ cho demo)",
                                        IsCorrect = true,
                                        Score = 10
                                    },
                                    new Answer
                                    {
                                        Content = "Answer 2",
                                        IsCorrect = false,
                                        Score = 0
                                    },
                                    new Answer
                                    {
                                        Content = "Answer 3",
                                        IsCorrect = false,
                                        Score = 0
                                    },
                                    new Answer
                                    {
                                        Content = "Answer 4",
                                        IsCorrect = false,
                                        Score = 0
                                    }
                                };
                            }

                            lo.Quiz = new Quiz
                            {
                                TimeLimit = lo.MaxLearningTime,
                                Questions = questions
                            };
                        }

                        _context.LearningObjects.Add(lo);
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

    public class LearningObjectRecord : ClassMap<LearningObject>
    {
        public LearningObjectRecord()
        {
            Map(x => x.Title).Name("Title");
            Map(x => x.TopicId).Name("TopicId").TypeConverter<Int32Converter>();
            //use enum converter
            //Map(x => x.Structure).Name("Structure").TypeConverter(new EnumConverter(typeof(Structure))).TypeConverterOption.EnumIgnoreCase(true);
            //Map(x => x.AggregationLevel).Name("AggregationLevel").TypeConverter(new EnumConverter(typeof(AggregationLevel))).TypeConverterOption.EnumIgnoreCase(true);
            //Map(x => x.Format).Name("Format").TypeConverter(new EnumConverter(typeof(Format))).TypeConverterOption.EnumIgnoreCase(true);
            //Map(x => x.LearningResourceType).Name("LearningResourceType").TypeConverter(new EnumConverter(typeof(LearningResourceType))).TypeConverterOption.EnumIgnoreCase(true);
            //Map(x => x.InteractivityType).Name("InteractivityType").TypeConverter(new EnumConverter(typeof(InteractivityType))).TypeConverterOption.EnumIgnoreCase(true);
            //Map(x => x.InteractivityLevel).Name("InteractivityLevel").TypeConverter(new EnumConverter(typeof(InteractivityLevel))).TypeConverterOption.EnumIgnoreCase(true);
            //Map(x => x.SemanticDensity).Name("SemanticDensity").TypeConverter(new EnumConverter(typeof(SemanticDensity))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.MaxScore).Name("MaxScore").TypeConverter<Int32Converter>();
            Map(x => x.MaxLearningTime).Name("MaxLearningTime").TypeConverter<Int32Converter>();
            Map(x => x.Type).Name("LOexev").TypeConverter(new EnumConverter(typeof(LOType))).TypeConverterOption.EnumIgnoreCase(true);
            Map(x => x.Difficulty).Name("Difficulty").TypeConverter<Int32Converter>();
        }
    }
}
