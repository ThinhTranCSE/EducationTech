using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class LearnerLog : Entity
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public int LearningObjectId { get; set; }
    public int Rating { get; set; }
    public virtual Learner Learner { get; set; }
    public virtual LearningObject LearningObject { get; set; }


    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<LearnerLog>(modelBuilder);
    }
}
