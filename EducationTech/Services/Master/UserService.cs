using EducationTech.Models.Master;
using EducationTech.Repositories.Master.Interfaces;
using EducationTech.Services.Master.Interfaces;

namespace EducationTech.Services.Master
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) { _userRepository = userRepository; }
        public async Task<User?> Get(int id)
        {
            return await _userRepository.Get(id);
        }
    }
}
