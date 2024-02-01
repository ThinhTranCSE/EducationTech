using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;

namespace EducationTech.Business.Repositories.Master.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserById(Guid id);
    }
}
