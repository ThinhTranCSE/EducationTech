using EducationTech.DTOs.Masters.User;
using EducationTech.Models.Master;
using EducationTech.Repositories.Abstract.Crud;
using EducationTech.Repositories.Abstracts;

namespace EducationTech.Repositories.Master.Interfaces
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
