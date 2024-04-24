using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Master
{
    public class UserRole : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<UserRole>(modelBuilder);
        }
    }
}
