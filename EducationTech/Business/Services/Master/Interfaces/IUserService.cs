using EducationTech.Business.Models.Master;
using EducationTech.Business.Services.Abstract;

namespace EducationTech.Business.Services.Master.Interfaces
{
    public interface IUserService : IService
    {
        Task<User?> GetUserById(Guid id);
    }
}
