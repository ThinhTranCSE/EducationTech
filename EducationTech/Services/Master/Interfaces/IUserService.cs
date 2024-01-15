using EducationTech.Models.Master;
using EducationTech.Services.Abstract;

namespace EducationTech.Services.Master.Interfaces
{
    public interface IUserService : IService
    {
        Task<User?> Get(int id);
    }
}
