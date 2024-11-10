using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business
{
    public class Quiz : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LearningObjectId { get; set; }
        public int TimeLimit { get; set; }

        public virtual LearningObject LearningObject { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Quiz>(modelBuilder);
        }
    }
}