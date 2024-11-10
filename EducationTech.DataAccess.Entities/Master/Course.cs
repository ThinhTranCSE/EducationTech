using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Master;

[Index(nameof(CourseCode), IsUnique = true)]
public class Course : Entity
{
    public override bool Timestamp => true;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public bool IsArchived { get; set; }
    public bool IsPublished { get; set; }
    public DateTime PublishedAt { get; set; }
    public string ImageUrl { get; set; }

    // new properties
    public string CourseCode { get; set; }
    public int Credits { get; set; }
    public int RecommendedSemester { get; set; }
    public int? CourseGroupId { get; set; }

    public virtual User Owner { get; set; }
    public virtual ICollection<CourseSection> CourseSections { get; set; } = new List<CourseSection>();
    public virtual ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
    public virtual ICollection<RecommendTopic> Topics { get; set; } = new List<RecommendTopic>();
    public virtual ICollection<PrerequisiteCourse> Prerequisites { get; set; } = new List<PrerequisiteCourse>();
    public virtual CourseGroup? CourseGroup { get; set; }
    public virtual ICollection<CourseSpeciality> Specialities { get; set; } = new List<CourseSpeciality>();

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<Course>(modelBuilder);

        // Configure the relationship between Course and PrerequisiteCourse
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Prerequisites)
            .WithOne(p => p.Course) // Assuming MainCourse is the main Course in PrerequisiteCourse
            .HasForeignKey(p => p.CourseId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
