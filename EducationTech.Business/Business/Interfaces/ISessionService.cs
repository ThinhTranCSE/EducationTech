using EducationTech.Business.Abstract;
using EducationTech.DataAccess.Entities.Master;

namespace EducationTech.Business.Business.Interfaces;

public interface ISessionService : IService
{
    User? CurrentUser { get; }
}
