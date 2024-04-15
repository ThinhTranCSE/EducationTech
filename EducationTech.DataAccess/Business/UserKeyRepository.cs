using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.DataAccess.Business
{
    public class UserKeyRepository : Repository<UserKey>, IUserKeyRepository
    {
        public UserKeyRepository(EducationTechContext context) : base(context)
        {
        }

        public override DbSet<UserKey> Model => _context.UserKeys;

    }
}
