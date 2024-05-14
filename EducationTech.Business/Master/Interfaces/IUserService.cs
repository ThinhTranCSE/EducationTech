using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.Users;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Master.Interfaces
{
    public interface IUserService : IService
    {
        Task<UserDto?> GetUserById(Guid id);
        Task<User?> UpdateUser(Guid userId, User_UpdateDto updateDto, User currentUser);
    }
}
