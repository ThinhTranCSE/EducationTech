using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business
{
    public class Comunity : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }


        public virtual Course Course { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Comunity>(modelBuilder);
        }
    }
}
