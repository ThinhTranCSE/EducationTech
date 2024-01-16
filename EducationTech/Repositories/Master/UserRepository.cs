using EducationTech.Databases;
using EducationTech.DTOs.Masters.User;
using EducationTech.Models.Master;
using EducationTech.Repositories.Abstract;
using EducationTech.Repositories.Master.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace EducationTech.Repositories.Master
{
    public class UserRepository : Repository, IUserRepository
    {
        public DbSet<User> model => _context.Users;
        public UserRepository(MainDatabaseContext context) : base(context) { }

        public async Task<ICollection<User>> Get(UserGetDto getDto)
        {
            var query = model.AsQueryable();
            if(getDto.Id != null)
            {
                query = query.Where(x => x.Id == getDto.Id);
            }
            if(getDto.Ids != null)
            {
                query = query.Where(x => getDto.Ids.Contains(x.Id));
            }
            if(getDto.Username != null)
            {
                query = query.Where(x => x.Username == getDto.Username);
            }

            return await query.ToListAsync();
        }

        public Task<User?> Insert(UserInsertDto insertDto)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<User>> Insert(IEnumerable<UserInsertDto> insertDtos)
        {
            throw new NotImplementedException();
        }

        public Task<User?> Update(UserUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<User?> Delete(UserDeleteDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
