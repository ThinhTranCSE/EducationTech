using Castle.Core.Internal;
using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Abstract;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using EducationTech.Exceptions.Http;
using EducationTech.Utilities;
using EducationTech.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;

namespace EducationTech.Business.Repositories.Master
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
