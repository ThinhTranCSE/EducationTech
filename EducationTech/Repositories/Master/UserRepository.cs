using EducationTech.Databases;
using EducationTech.DTOs.Masters.User;
using EducationTech.Models.Master;
using EducationTech.Repositories.Abstract;
using EducationTech.Repositories.Master.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationTech.Repositories.Master
{
    public class UserRepository : Repository, IUserRepository
    {
        public DbSet<User> Query => _context.Users;
        public UserRepository(MainDatabaseContext context) : base(context) { }

        public async Task<User?> Get(int id)
        {
            return new User { Username = "Test username", DateOfBirth = new DateTime(2022, 12, 5)};
        }

        public Task<User?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User?> Insert(UserInsertDto model)
        {
            return Insert<UserInsertDto>(model);
        }

        public Task<User?> Insert<TInsertDto>(TInsertDto model)
        {
            throw new NotImplementedException();
        }

        public Task<User?> Update(int id, UserUpdateDto model)
        {
            return Update<UserUpdateDto>(id, model);
        }

        public Task<User?> Update<TUpdateDto>(int id, TUpdateDto model)
        {
            throw new NotImplementedException();
        }
        public Task<User?> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User[]> Get<TGetDto>(TGetDto model)
        {
            throw new NotImplementedException();
        }
    }
}
