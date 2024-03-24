using EducationTech.Business.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.Business.Models.Master
{
    public class UploadedFile : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string OriginalFileName { get; set; }
        public long Size { get; set; }
        public string Path { get; set; } = "temp path";
        public bool IsCompleted { get; set; }

        public bool IsPublic { get; set; }  

        public Guid UserId { get; set; } = new Guid("41805b8c-9a7f-4fc7-9487-08dc20ae4bf8");

        public virtual User User { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<UploadedFile>(modelBuilder);
        }
    }
}
