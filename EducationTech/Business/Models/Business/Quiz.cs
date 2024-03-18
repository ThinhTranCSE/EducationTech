using EducationTech.Business.Models.Abstract;
using EducationTech.Business.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Business.Models.Business
{
    public class Quiz : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int TimeLimit { get; set; }


        public virtual Lesson Lesson { get; set; }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Quiz>(modelBuilder);
        }
    }
}