using EducationTech.DTOs.Masters.User;
using EducationTech.Models.Master;
using EducationTech.Repositories.Abstracts;

namespace EducationTech.Repositories.Master.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsername(string username);
        Task<User?> GetById(int id);
        Task<User?> Insert(UserInsertDto model);
        Task<User?> Update(int id, UserUpdateDto model);
    }
}
