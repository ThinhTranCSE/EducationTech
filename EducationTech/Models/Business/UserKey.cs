using EducationTech.Models.Abstract;
using EducationTech.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Models.Business
{
    public class UserKey : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string PublicKey { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<UserKey>(modelBuilder);
        }
    }
}
