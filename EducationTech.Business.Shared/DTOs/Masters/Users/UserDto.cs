using EducationTech.Business.Shared.DTOs.Masters.UserRoles;
using EducationTech.Business.Shared.DTOs.Recommendation.Learners;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Shared.DTOs.Masters.Users
{
    public class UserDto : Abstracts.AbstractDto<User, UserDto>
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = "";

        public string? Email { get; set; }

        public ICollection<UserRoleDto> UserRoles { get; set; }
        public LearnerDto? Learner { get; set; }
    }
}
