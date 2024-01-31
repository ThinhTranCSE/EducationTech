using EducationTech.Business.DTOs.Business.UserKey;
using EducationTech.Business.Models.Business;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Databases;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Business.Repositories.Business
{
    public class UserKeyRepository : Repository, IUserKeyRepository
    {
        public UserKeyRepository(MainDatabaseContext context) : base(context)
        {
        }

        public DbSet<UserKey> Model => _context.UserKeys;


        public async Task<UserKey?> Insert(UserKey_InsertDto insertDto)
        {
            var userKey = await Model.AddAsync(new UserKey
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
