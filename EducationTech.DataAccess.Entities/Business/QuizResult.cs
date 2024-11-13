using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationTech.DataAccess.Entities.Business;

public class QuizResult : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int QuizId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Score { get; set; }
    public int? TimeTaken { get; set; }
    public virtual Quiz Quiz { get; set; }
    public virtual User User { get; set; }
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<QuizResult>(modelBuilder);
    }
}
