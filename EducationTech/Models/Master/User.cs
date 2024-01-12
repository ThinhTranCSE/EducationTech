using EducationTech.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Models.Master
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(200, MinimumLength = 8, ErrorMessage = "Password must in range (8, 200)")]
        [Column("Password")]
        public string HashedPassword { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        public Role Role { get; set; } = Role.Student;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }

}
