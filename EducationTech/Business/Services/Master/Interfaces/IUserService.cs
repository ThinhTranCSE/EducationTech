using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Services.Abstract;

namespace EducationTech.Business.Services.Master.Interfaces
{
    public interface IUserService : IService
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> UpdateUser(Guid userId, User_UpdateDto updateDto, User currentUser);
    }
}
