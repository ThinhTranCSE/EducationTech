using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class PrerequisiteCourse : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int PrerequisiteCourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; }

    [ForeignKey(nameof(PrerequisiteCourseId))]
    public virtual Course Prerequisite { get; set; }
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<PrerequisiteCourse>(modelBuilder);

        //modelBuilder.Entity<PrerequisiteCourse>()
        //    .HasOne(p => p.Course)
        //    .WithMany()
        //    .HasForeignKey(p => p.CourseId)
        //    .OnDelete(DeleteBehavior.NoAction);

        //modelBuilder.Entity<PrerequisiteCourse>()
        //    .HasOne(p => p.Prerequisite)
        //    .WithMany()
        //    .HasForeignKey(p => p.PrerequisiteCourseId)
        //    .OnDelete(DeleteBehavior.NoAction);
    }
}
