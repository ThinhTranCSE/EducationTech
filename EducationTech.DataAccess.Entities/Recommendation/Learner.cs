using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Shared.Enums.Learner;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class Learner : Entity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; }
    public Gender Gender { get; set; }
    public virtual LearningStyle LearningStyle { get; set; }
    public BackgroundKnowledge BackgroundKnowledge { get; set; }
    public Qualification Qualification { get; set; }
    public string Branch { get; set; }

    public virtual ICollection<LearnerLog> LearnerLogs { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<Learner>(modelBuilder);
    }
}
