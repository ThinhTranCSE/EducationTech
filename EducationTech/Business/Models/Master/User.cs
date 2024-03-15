using EducationTech.Business.Models.Abstract;
using EducationTech.Business.Models.Business;
using EducationTech.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public byte[] Salt { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual UserKey? UserKey { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<User>(modelBuilder);

        }
    }
}
