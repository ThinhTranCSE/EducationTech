using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

[Index(nameof(TopicId), nameof(LearnerId), IsUnique = true)]
public class TopicLearningPathOrder : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int TopicId { get; set; }
    public int LearnerId { get; set; }
    public int Order { get; set; }

    public virtual RecommendTopic Topic { get; set; }
    public virtual Learner Learner { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<TopicLearningPathOrder>(modelBuilder);
    }
}
