using EducationTech.DataAccess.Entities.Abstract;
using EducationTech.DataAccess.Shared.Enums.LearningObject;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Entities.Recommendation;

public class LearningObject : Entity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Structure Structure { get; set; }
    public AggregationLevel AggregationLevel { get; set; }
    public Format Format { get; set; }
    public LearningResourceType LearningResourceType { get; set; }
    public InteractivityType InteractivityType { get; set; }
    public InteractivityLevel InteractivityLevel { get; set; }
    public SemanticDensity SemanticDensity { get; set; }

    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSideEffects<LearningObject>(modelBuilder);
    }
}
