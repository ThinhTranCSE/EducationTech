using EducationTech.Business.Models.Abstract;
using EducationTech.Business.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Business.Models.Business
{
    public class InstructorApproved : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid AdminId { get; set; }
        public virtual User Admin { get; set; }

        public Guid InstructorId { get; set; }
        public virtual User Instructor { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<InstructorApproved>(modelBuilder);
        }
    }
}
