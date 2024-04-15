using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Master
{
    public class Course : Model
    {
        public override bool Timestamp => true;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public bool IsArchived { get; set; }

        public double Price { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Course>(modelBuilder);
        }
    }
}
