using CsvHelper;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Storage;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class RecommendTopicSeeder : Seeder
    {
        public RecommendTopicSeeder(EducationTechContext context) : base(context)
        {
        }

        public override void Seed()
        {
            var globalUsings = GlobalReference.Instance;
            using (var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "Topics.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<TopicRecord>();
                _context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS=0;");
                foreach (var record in records)
                {
                    if (_context.RecommendTopics.Any(x => x.Name == record.Name))
                    {
                        continue;
                    }
                    var topic = new RecommendTopic
                    {
                        Name = record.Name
                    };

                    _context.RecommendTopics.Add(topic);
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(record.NextTopicIds))
                    {
                        var nextTopicIds = record.NextTopicIds.Trim('[', ']').Split(", ");
                        foreach (var nextTopicId in nextTopicIds)
                        {
                            var topicConjunction = new TopicConjunction
                            {
                                TopicId = topic.Id,
                                NextTopicId = Convert.ToInt32(nextTopicId)
                            };
                            _context.TopicConjunctions.Add(topicConjunction);
                        }
                        _context.SaveChanges();
                    }
                }
                _context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS=1;");
            }
        }

    }

    public class TopicRecord
    {
        public string Name { get; set; }
        public string NextTopicIds { get; set; }
    }
}

