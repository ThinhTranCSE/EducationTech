using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EducationTech.DataAccess.Entities.Master
{
    public class Role : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Role must have specific name")]
        public string Name { get; set; } = "";

        [IgnoreDataMember]
        public virtual ICollection<User> Users { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Role>(modelBuilder);
            modelBuilder.Entity<Role>()
                .HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .UsingEntity<UserRole>(
                    ur => ur.HasOne(x => x.User).WithMany(x => x.UserRoles),
                    ur => ur.HasOne(x => x.Role).WithMany(x => x.UserRoles)
                )
                .ToTable(nameof(UserRoles));
        }
    }
}
