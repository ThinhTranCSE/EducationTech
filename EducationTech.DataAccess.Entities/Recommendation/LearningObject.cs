using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Business;
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
    public int Order { get; set; }
    public int Difficulty { get; set; }
    public int MaxScore { get; set; }
    public int MaxLearningTime { get; set; }
    public LOType Type { get; set; }
    public virtual RecommendTopic Topic { get; set; }
    public virtual Video? Video { get; set; }
    public virtual Quiz? Quiz { get; set; }

    public virtual ICollection<LearnerLog> LearnerLogs { get; set; }
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<LearningObject>(modelBuilder);

    }
}
