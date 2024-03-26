using EducationTech.Business.Models.Abstract;
using EducationTech.Business.Models.Business;
using EducationTech.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Pqc.Crypto.Bike;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EducationTech.Business.Models.Master
{
    [Index(nameof(Username), IsUnique = true)]
    public class User : Model
    {
        public override bool Timestamp => true;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Username { get; set; } = "";

        [JsonIgnore]
        public string Password { get; set; } = "";

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [JsonIgnore]
        public byte[] Salt { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

        [IgnoreDataMember]
        public virtual UserKey UserKey { get; set; } = null!;

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
