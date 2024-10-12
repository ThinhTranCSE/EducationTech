using CsvHelper;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Storage;
using Microsoft.EntityFrameworkCore;
using Neo4jClient;
using System.Globalization;

namespace EducationTech.DataAccess.Seeders.Seeds
{
    public class RecommendTopicSeeder : Seeder
    {
        private readonly IGraphClient _graphClient;
        public RecommendTopicSeeder(EducationTechContext context, IGraphClient graphClient) : base(context)
        {
            _graphClient = graphClient;
        }

        public override void Seed()
        {
            using var transaction = _context.Database.BeginTransaction();
            using var graphTransaction = _graphClient.Tx.BeginTransaction();
            try
            {
                var globalUsings = GlobalReference.Instance;
                using var reader = new StreamReader(Path.Combine(globalUsings.StaticFilesPath, "Topics.csv"));
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<TopicRecord>();
                _context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS=0;");
                foreach (var record in records)
                {
                    if (_context.RecommendTopics.Any(x => x.Name == record.Name))
                    {
                        continue;
                    }
                    var createdTopic = new RecommendTopic
                    {
                        CourseId = int.Parse(record.CourseId),
                        Name = record.Name
                    };

                    _context.RecommendTopics.Add(createdTopic);
                    _context.SaveChanges();

                    //nếu topic có id tồn tại thì update, không thì insert
                    _graphClient.Cypher
                        .Merge("(topic:Topic { Id: $id })")
                        .OnCreate()
                        .Set("topic = $createdTopic")
                        .OnMatch()
                        .Set("topic = $createdTopic")
                        .WithParams(new
                        {
                            id = createdTopic.Id,
                            createdTopic
                        })
                        .ExecuteWithoutResultsAsync().Wait();



                    if (!string.IsNullOrEmpty(record.NextTopicIds))
                    {
                        var nextTopicIds = record.NextTopicIds != "[]" ? record.NextTopicIds.Trim('[', ']').Split(", ") : Array.Empty<string>();
                        foreach (var nextTopicId in nextTopicIds)
                        {
                            var topicConjunction = new TopicConjunction
                            {
                                TopicId = createdTopic.Id,
                                NextTopicId = Convert.ToInt32(nextTopicId)
                            };
                            _context.TopicConjunctions.Add(topicConjunction);

                            _graphClient.Cypher
                                .Merge("(topic:Topic { Id: $id })")
                                .OnCreate()
                                .Set("topic = $createdTopic")
                                .WithParams(new
                                {
                                    id = topicConjunction.NextTopicId,
                                    createdTopic = new RecommendTopic { Id = topicConjunction.NextTopicId }
                                })
                                .ExecuteWithoutResultsAsync().Wait();

                            _graphClient.Cypher
                                .Match("(topic1:Topic)", "(topic2:Topic)")
                                .Where((RecommendTopic topic1) => topic1.Id == topicConjunction.TopicId)
                                .AndWhere((RecommendTopic topic2) => topic2.Id == topicConjunction.NextTopicId)
                                .Merge("(topic1)-[:LEADS_TO]->(topic2)")
                                .ExecuteWithoutResultsAsync().Wait();
                        }
                        _context.SaveChanges();
                    }
                }
                transaction.Commit();
                graphTransaction.CommitAsync().Wait();
                _context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS=1;");
            }
            catch
            {
                transaction.Rollback();
                graphTransaction.RollbackAsync().Wait();
                throw;
            }
        }

    }

    public class TopicRecord
    {
        public string CourseId { get; set; }
        public string Name { get; set; }
        public string NextTopicIds { get; set; }
    }
}

