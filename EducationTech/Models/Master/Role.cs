using EducationTech.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Models.Master
{
    public class Role : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey(nameof(UserRole))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Role must have specific name")]
        public string Name { get; set; } = "";

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Role>(modelBuilder);
        }
    }
}
