using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Masters.User;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Master.Interfaces
{
    public interface IUserService : IService
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> UpdateUser(Guid userId, User_UpdateDto updateDto, User currentUser);
    }
}
