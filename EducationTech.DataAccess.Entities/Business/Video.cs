using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business
{
    public class Video : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? LearningObjectId { get; set; }
        public string Url { get; set; } = null!;
        public Guid FileId { get; set; }
        public virtual LearningObject? LearningObject { get; set; } = null!;
        public virtual UploadedFile File { get; set; } = null!;
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Video>(modelBuilder);
        }
    }
}
