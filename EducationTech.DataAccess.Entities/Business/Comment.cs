using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Shared.NestedSet;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business
{
    public class Comment : Entity, INestedSetNode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public Guid OwnerId { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int? RepliedCommentId { get; set; }
        public int DiscussionId { get; set; }

        public virtual User Owner { get; set; }
        public virtual Discussion Discussion { get; set; }
        public virtual Comment RepliedComment { get; set; }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureSideEffects<Comment>(modelBuilder);
        }
    }
}
