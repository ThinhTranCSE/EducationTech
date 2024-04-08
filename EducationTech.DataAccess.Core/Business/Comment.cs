using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business
{
    public class Comment : Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TopicId { get; set; }

        public Guid UserId { get; set; }

        public int RepliedCommentId { get; set; }

        public string Content { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual Comment RepliedComment { get; set; }

        public virtual User User { get; set; }


        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Comment>(modelBuilder);
        }
    }
}
