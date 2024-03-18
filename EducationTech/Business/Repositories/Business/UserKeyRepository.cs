using EducationTech.Business.DTOs.Business.UserKey;
using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class UserKeyRepository : Repository<UserKey>, IUserKeyRepository
    {
        public UserKeyRepository(EducationTechContext context) : base(context)
        {
        }

        public override DbSet<UserKey> Model => _context.UserKeys;

    }
}
