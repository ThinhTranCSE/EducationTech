using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class CourseGroup : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int MinCredits { get; set; }
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<CourseGroup>(modelBuilder);
    }
}
