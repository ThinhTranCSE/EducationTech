using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.Shared.Utilities.Interfaces;

namespace EducationTech.DataAccess.Master;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly IEncryptionUtils _encryptionUtils;

    public UserRepository(IMainDatabaseContext context, IEncryptionUtils encryptionUtils) : base(context)
    {
        _encryptionUtils = encryptionUtils;
    }
}
