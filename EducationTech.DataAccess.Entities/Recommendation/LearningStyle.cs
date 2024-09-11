using EducationTech.DataAccess.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class LearningStyle : Entity
{
    public int Id { get; set; }
    public int Active { get; set; }
    public int Reflective { get; set; }
    public int Sensing { get; set; }
    public int Intuitive { get; set; }
    public int Visual { get; set; }
    public int Verbal { get; set; }
    public int Sequential { get; set; }
    public int Global { get; set; }
    public int LearnerId { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<LearningStyle>(modelBuilder);
    }
}
