using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Business.Services.Master.Interfaces;
using EducationTech.Databases;
using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Services.Abstract;
using EducationTech.Utilities.Interfaces;

namespace EducationTech.Business.Services.Master
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository
            )
        {
            _userRepository = userRepository;
        }


        public async Task<User?> GetUserById(Guid id)
        {
            var users = await _userRepository.Get(new User_GetDto { Id = id });
            return users.Count > 0 ? users.First() : null;
        }
    }
}
