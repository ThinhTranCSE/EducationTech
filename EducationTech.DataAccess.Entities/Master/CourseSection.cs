using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Master
{
    public class CourseSection : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; }
        public int Order { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();


        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<CourseSection>(modelBuilder);
        }
    }
}
