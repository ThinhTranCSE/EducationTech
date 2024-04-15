using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;

namespace EducationTech.DataAccess.Master
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IEncryptionUtils _encryptionUtils;
        public override DbSet<User> Model => _context.Users;


        public UserRepository(EducationTechContext context, IEncryptionUtils encryptionUtils) : base(context)
        {
            _encryptionUtils = encryptionUtils;

        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var users = (await Get(x => x.Username == username))
                .Include(x => x.UserKey)
                .Include(x => x.UserRoles)
                    .ThenInclude(r => r.Role);


            return await users.FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var users = (await Get(x => Guid.Equals(x.Id, id)))
                .Include(x => x.UserKey)
                .Include(x => x.UserRoles)
                    .ThenInclude(r => r.Role);
            return await users.FirstOrDefaultAsync();
        }


    }
}
