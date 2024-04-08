using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.DataAccess.Master.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserById(Guid id);
    }
}
