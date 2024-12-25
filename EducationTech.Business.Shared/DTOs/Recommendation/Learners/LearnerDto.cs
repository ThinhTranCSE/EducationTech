using EducationTech.Business.Shared.DTOs.Abstracts;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;

namespace EducationTech.Business.Shared.DTOs.Recommendation.Learners;

public class LearnerDto : AbstractDto<Learner, LearnerDto>
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int SpecialityId { get; set; }
    public string IdentificationNumber { get; set; }
    public virtual Speciality Speciality { get; set; } = null!;
    public virtual UserInfo User { get; set; } = null!;
}

public class UserInfo : AbstractDto<User, UserInfo>
{
    public string Username { get; set; } = "";
    public string? Email { get; set; }
}
