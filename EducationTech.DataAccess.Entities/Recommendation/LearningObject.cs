using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class LearningObject : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int TopicId { get; set; }
    public string Title { get; set; }
    public Structure Structure { get; set; }
    public AggregationLevel AggregationLevel { get; set; }
    public Format Format { get; set; }
    public LearningResourceType LearningResourceType { get; set; }
    public InteractivityType InteractivityType { get; set; }
    public InteractivityLevel InteractivityLevel { get; set; }
    public SemanticDensity SemanticDensity { get; set; }

    public int Difficulty { get; set; }
    public int MaxScore { get; set; }
    public int MaxLearningTime { get; set; }
    public LOType Type { get; set; }
    public virtual RecommendTopic Topic { get; set; }

    public virtual ICollection<LearnerLog> LearnerLogs { get; set; }
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<LearningObject>(modelBuilder);
    }
}
