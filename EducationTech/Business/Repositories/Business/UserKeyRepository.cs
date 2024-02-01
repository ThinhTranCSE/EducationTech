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

        public Task<UserKey?> Delete(UserKey_DeleteDto deleteDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserKey?> Delete(UserKey deleteObject)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserKey>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<UserKey?> Insert(UserKey_InsertDto insertDto)
        {
            var userKey = new UserKey
            {
                PublicKey = insertDto.PublicKey,
                UserId = insertDto.UserId
            };
            await Model.AddAsync(userKey);

            return userKey;
        }
        public Task<IEnumerable<UserKey>> Insert(IEnumerable<UserKey_InsertDto> insertDtos)
        {
            throw new NotImplementedException();
        }

        public Task<UserKey?> Insert(UserKey insertObject)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserKey>> Insert(IEnumerable<UserKey> insertObjects)
        {
            throw new NotImplementedException();
        }

        public Task<UserKey?> Update(UserKey_UpdateDto updateDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserKey?> Update(UserKey updateObject)
        {
            throw new NotImplementedException();
        }
    }
}
