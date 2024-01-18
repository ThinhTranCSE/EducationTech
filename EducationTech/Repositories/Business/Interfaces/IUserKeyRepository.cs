using EducationTech.DTOs.Business.UserKey;
using EducationTech.Models.Business;
using EducationTech.Repositories.Abstract.Crud;
using EducationTech.Repositories.Abstracts;

namespace EducationTech.Repositories.Business.Interfaces
{
    public interface IUserKeyRepository : IRepository<UserKey>, IInsert<UserKey, UserKey_InsertDto>
    {

    }
}
