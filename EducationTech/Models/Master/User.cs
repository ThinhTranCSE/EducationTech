using EducationTech.Enums;
using EducationTech.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Models.Master
{
    [Index(nameof(Username), IsUnique = true)]
    public class User : Model
    {
        public override bool Timestamp => true;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey(nameof(UserRole))]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [JsonIgnore]
        public string Password { get; set; } = "";

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }

        [JsonIgnore]
        public string? AccessToken { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<User>(modelBuilder);
        }
    }
}
