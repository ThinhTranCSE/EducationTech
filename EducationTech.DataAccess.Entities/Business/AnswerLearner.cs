using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business
{
    public class AnswerLearner : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LearnerId { get; set; }
        public int AnswerId { get; set; }
        public int QuizResultId { get; set; }

        public virtual Learner Learner { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual QuizResult QuizResult { get; set; }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<AnswerLearner>(modelBuilder);
        }
    }
}
