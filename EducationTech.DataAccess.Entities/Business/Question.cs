using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business
{
    public class Question : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Content { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Question>(modelBuilder);
        }
    }
}
