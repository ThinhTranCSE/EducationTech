using EducationTech.Business.Models.Abstract;
using EducationTech.Business.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Business.Models.Business
{
    public class Topic : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ComunityId { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; } 

        public virtual User User { get; set; }
        public virtual Comunity Comunity { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Topic>(modelBuilder);
        }
    }
}
