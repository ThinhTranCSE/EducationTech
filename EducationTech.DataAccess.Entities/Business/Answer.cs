using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business
{
    public class Answer : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public virtual Question Question { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Answer>(modelBuilder);
        }
    }
}
