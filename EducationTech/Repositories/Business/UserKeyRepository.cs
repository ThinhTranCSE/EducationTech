using EducationTech.Databases;
using EducationTech.DTOs.Business.UserKey;
using EducationTech.Models.Business;
using EducationTech.Repositories.Abstract;
using EducationTech.Repositories.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Repositories.Business
{
    public class UserKeyRepository : Repository, IUserKeyRepository
    {
        public UserKeyRepository(MainDatabaseContext context) : base(context)
        {
        }

        public DbSet<UserKey> model => _context.UserKeys;


        public async Task<UserKey?> Insert(UserKey_InsertDto insertDto)
        {
            var userKey = await model.AddAsync(new UserKey
            {
                PublicKey = insertDto.PublicKey,
                UserId = insertDto.UserId
            });
            return userKey.Entity;
        }
        public Task<ICollection<UserKey>> Insert(IEnumerable<UserKey_InsertDto> insertDtos)
        {
            throw new NotImplementedException();
        }
    }
}
