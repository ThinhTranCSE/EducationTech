using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.Learners;

public class LearnerDto : AbstractDto<Learner, LearnerDto>
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int SpecialityId { get; set; }
    public virtual Speciality Speciality { get; set; } = null!;
}
