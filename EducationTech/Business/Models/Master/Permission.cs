using EducationTech.Business.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Business.Models.Master
{
    public class Permission : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Permission>(modelBuilder);
        }
    }
}
