using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

[Index(nameof(CourseId), nameof(LearnerId), IsUnique = true)]
public class CourseLearningPathOrder : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CourseId { get; set; }
    public int LearnerId { get; set; }
    public int Order { get; set; }
    public int Semester { get; set; }
    public virtual Course Course { get; set; }
    public virtual Learner Learner { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<CourseLearningPathOrder>(modelBuilder);
    }
}
