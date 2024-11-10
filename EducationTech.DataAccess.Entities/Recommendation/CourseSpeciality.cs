using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class CourseSpeciality : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int SpecialityId { get; set; }
    public virtual Course Course { get; set; }
    public virtual Speciality Speciality { get; set; }
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<CourseSpeciality>(modelBuilder);
    }
}
