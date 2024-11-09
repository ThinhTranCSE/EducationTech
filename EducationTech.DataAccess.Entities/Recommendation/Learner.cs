using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class Learner : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public int SpecialityId { get; set; }
    public virtual Speciality Speciality { get; set; } = null!;
    public virtual ICollection<LearnerLog> LearnerLogs { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<Learner>(modelBuilder);
    }
}
