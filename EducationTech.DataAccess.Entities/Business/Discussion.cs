using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business;

public class Discussion : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public Guid OwnerId { get; set; }
    public int ComunityId { get; set; }

    public virtual User Owner { get; set; }
    public virtual Comunity Comunity { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<Discussion>(modelBuilder);
    }
}
