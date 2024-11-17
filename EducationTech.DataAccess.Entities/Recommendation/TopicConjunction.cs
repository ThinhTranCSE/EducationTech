//using EducationTech.DataAccess.Entities.Abstract;
//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace EducationTech.DataAccess.Entities.Recommendation;

//public class TopicConjunction : Entity
//{
//    [Key]
//    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//    public int Id { get; set; }

//    public int TopicId { get; set; }

//    public int NextTopicId { get; set; }

//    public virtual RecommendTopic Topic { get; set; }
//    public virtual RecommendTopic NextTopic { get; set; }

//    public override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        ConfigureSideEffects<TopicConjunction>(modelBuilder);

//        //modelBuilder.Entity<TopicConjunction>()
//        //    .HasOne(x => x.Topic)
//        //    .WithMany(x => x.NextTopicConjuctions)
//        //    .HasForeignKey(x => x.TopicId);

//        //modelBuilder.Entity<TopicConjunction>()
//        //    .HasOne(x => x.NextTopic)
//        //    .WithMany()
//        //    .HasForeignKey(x => x.NextTopicId);
//    }
//}
