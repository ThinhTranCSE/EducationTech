using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class Branch : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Speciality> Specialities { get; set; } = new List<Speciality>();

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<Branch>(modelBuilder);
    }
}
