using EducationTech.Business.DTOs.Business.UserKey;
using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Abstract.Crud;

namespace EducationTech.Business.Repositories.Business.Interfaces
{
    public interface IUserKeyRepository : IRepository<UserKey>, IInsert<UserKey, UserKey_InsertDto>
    {

    }
}
