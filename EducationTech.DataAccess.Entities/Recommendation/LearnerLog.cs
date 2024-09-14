using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class LearnerLog : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public int LearningObjectId { get; set; }
    public int Rating { get; set; }
    public int Score { get; set; }
    public int Attempt { get; set; }
    public int TimeTaken { get; set; }
    public DateTime VisitedAt { get; set; }
    public int VisitedTime { get; set; }

    public virtual Learner Learner { get; set; }
    public virtual LearningObject LearningObject { get; set; }


    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<LearnerLog>(modelBuilder);
    }
}
