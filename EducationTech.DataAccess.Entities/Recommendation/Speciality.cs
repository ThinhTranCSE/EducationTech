using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class Speciality : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int BranchId { get; set; }
    public virtual Branch Branch { get; set; }
    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<Speciality>(modelBuilder);
    }
}
