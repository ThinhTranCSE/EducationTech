using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Master
{
    public class Course : Entity
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

        public bool IsPublished { get; set; }

        public DateTime PublishedAt { get; set; }

        public double Price { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<LearnerCourse> LearnerCourses { get; set; } = new List<LearnerCourse>();
        public virtual ICollection<CourseSection> CourseSections { get; set; } = new List<CourseSection>();
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Course>(modelBuilder);
        }
    }
}
