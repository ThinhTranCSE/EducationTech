using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class LearningStyle : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public float Active { get; set; }
    public float Reflective { get; set; }
    public float Sensing { get; set; }
    public float Intuitive { get; set; }
    public float Visual { get; set; }
    public float Verbal { get; set; }
    public float Sequential { get; set; }
    public float Global { get; set; }

    public virtual Learner Learner { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<LearningStyle>(modelBuilder);
    }
}
