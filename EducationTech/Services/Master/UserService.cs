using EducationTech.DTOs.Masters.User;
using EducationTech.Models.Master;
using EducationTech.Repositories.Master.Interfaces;
using EducationTech.Services.Master.Interfaces;

namespace EducationTech.Services.Master
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) { _userRepository = userRepository; }
        public async Task<User?> GetUserById(Guid id)
        {
            var users = await _userRepository.Get(new User_GetDto { Id = id });
            return users.Count() > 0 ? users.First() : null;
        }
    }
}
