using EducationTech.Business.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Business.Models.Master
{
    public class Lesson : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseSectionId { get; set; }

        public string Title { get; set; }

        public int Order { get; set; }

        public string Type { get; set; }
        public virtual CourseSection CourseSection { get; set; }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Lesson>(modelBuilder);
        }
    }
}
