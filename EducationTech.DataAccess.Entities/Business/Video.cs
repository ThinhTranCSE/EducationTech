using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
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
        public int? LessonId { get; set; }
        public string Url { get; set; }
        public Guid FileId { get; set; }
        public virtual Lesson? Lesson { get; set; }
        public virtual UploadedFile File { get; set; }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Video>(modelBuilder);
        }
    }
}
