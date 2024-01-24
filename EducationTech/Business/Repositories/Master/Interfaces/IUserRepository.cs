using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Abstract.Crud;

namespace EducationTech.Business.Repositories.Master.Interfaces
{
    public interface IUserRepository :
        IRepository<User>,
        IGet<User, User_GetDto>,
        IInsert<User, User_InsertDto>,
        IUpdate<User, User_UpdateDto>,
        IDelete<User, User_DeleteDto>
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserById(Guid id);
    }
}
