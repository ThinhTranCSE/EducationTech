using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Master
{
    [Index(nameof(Username), IsUnique = true)]
    public class User : Entity
    {
        public override bool Timestamp => true;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Username { get; set; } = "";
        [JsonIgnore]
        public string Password { get; set; } = "";
        public string? Email { get; set; }
        [JsonIgnore]
        public byte[] Salt { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual UserKey UserKey { get; set; } = null!;
        public virtual Learner? Learner { get; set; } = null!;
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<User>(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<UserRole>(
                    ur => ur.HasOne(x => x.Role).WithMany(x => x.UserRoles),
                    ur => ur.HasOne(x => x.User).WithMany(x => x.UserRoles)
                )
                .ToTable(nameof(UserRoles));


        }
    }
}
