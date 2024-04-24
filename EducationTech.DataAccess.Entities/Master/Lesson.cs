using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Master
{
    public class Lesson : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CourseSectionId { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public LessonType Type { get; set; }
        public virtual CourseSection CourseSection { get; set; }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Lesson>(modelBuilder);
        }
    }
}
