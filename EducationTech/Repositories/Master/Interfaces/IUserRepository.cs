using EducationTech.DTOs.Masters.User;
using EducationTech.Models.Master;
using EducationTech.Repositories.Abstract.Crud;
using EducationTech.Repositories.Abstracts;

namespace EducationTech.Repositories.Master.Interfaces
{
    public interface IUserRepository : 
        IRepository<User>, 
        IGet<User, UserGetDto>, 
        IInsert<User, UserInsertDto>, 
        IUpdate<User, UserUpdateDto>, 
        IDelete<User, UserDeleteDto>
    {

    }
}
