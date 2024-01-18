using EducationTech.Databases;
using EducationTech.DTOs.Masters.User;
using EducationTech.Exceptions.Http;
using EducationTech.Models.Master;
using EducationTech.Repositories.Abstract;
using EducationTech.Repositories.Master.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;

namespace EducationTech.Repositories.Master
{
    public class UserRepository : Repository, IUserRepository
    {
        public DbSet<User> model => _context.Users;
        public UserRepository(MainDatabaseContext context) : base(context) { }

        public async Task<ICollection<User>> Get(User_GetDto getDto)
        {
            var query = model
                .AsQueryable();
                
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

            return await query
                .Include(x => x.UserRoles)
                .Include(x => x.UserKey)
                .ToListAsync();
        }

        public async Task<User?> Insert(User_InsertDto insertDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model.Any(u => u.Username == insertDto.Username))
                    {
                        throw new HttpException(HttpStatusCode.Conflict, "Username already exists");
                    }
                    User user = new User()
                    {
                        Username = insertDto.Username,
                        Password = insertDto.Password,
                        Salt = insertDto.Salt,
                        DateOfBirth = insertDto.DateOfBirth,
                        Email = insertDto.Email,
                        PhoneNumber = insertDto.PhoneNumber
                    };

                    var userInsert = model.Add(user);
                    _context.SaveChanges();
                    transaction.Commit();

                    return userInsert.Entity;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Task<ICollection<User>> Insert(IEnumerable<User_InsertDto> insertDtos)
        {
            throw new NotImplementedException();
        }

        public Task<User?> Update(User_UpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<User?> Delete(User_DeleteDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var users = await Get(new User_GetDto()
            {
                Username = username
            });

            return users.FirstOrDefault();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var users = await Get(new User_GetDto()
            {
                Id = id
            });

            return users.FirstOrDefault();
        }
    }
}
