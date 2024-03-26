using EducationTech.Business.Models.Abstract;
using EducationTech.Business.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Business.Models.Business
{
    public class LearnerCourse : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid LearnerId { get; set; }
        public virtual User Learner { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public double Rate { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<LearnerCourse>(modelBuilder);
        }
    }
}
