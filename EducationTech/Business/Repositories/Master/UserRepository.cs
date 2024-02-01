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
    public class UserRepository : Repository, IUserRepository
    {
        private readonly IEncryptionUtils _encryptionUtils;
        public DbSet<User> Model => _context.Users;


        public UserRepository(MainDatabaseContext context, IEncryptionUtils encryptionUtils) : base(context)
        {
            _encryptionUtils = encryptionUtils;
        }

        public async Task<IEnumerable<User>> Get()
        {
            //var query = Model
            //    .AsQueryable();

            //if (getDto.Id != null)
            //{
            //    query = query.Where(x => x.Id == getDto.Id);
            //}
            //if (getDto.Ids != null)
            //{
            //    query = query.Where(x => getDto.Ids.Contains(x.Id));
            //}
            //if (getDto.Username != null)
            //{
            //    query = query.Where(x => x.Username == getDto.Username);
            //}
            //if (getDto.IsIncludeRoles)
            //{
            //    query = query.Include(x => x.UserRoles)
            //            .ThenInclude(r => r.Role);
            //}
            //if (getDto.IsIncludeKey)
            //{
            //    query = query.Include(x => x.UserKey);
            //}
            //return query;
            return Model;

        }

        public async Task<User?> Insert(User_InsertDto insertDto)
        {
            User user = new User
            {
                Username = insertDto.Username,
                Password = _encryptionUtils.HashPassword(insertDto.Password, out var salt),
                Salt = salt,
                DateOfBirth = insertDto.DateOfBirth,
                Email = insertDto.Email,
                PhoneNumber = insertDto.PhoneNumber
            };

            await Model.AddAsync(user);
            return user;
        }

        public async Task<IEnumerable<User>> Insert(IEnumerable<User_InsertDto> insertDtos)
        {
            IEnumerable<User> users = insertDtos.Select(x => new User()
            {
                Username = x.Username,
                Password = _encryptionUtils.HashPassword(x.Password, out var salt),
                Salt = salt,
                DateOfBirth = x.DateOfBirth,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber
            });

            await Model.AddRangeAsync(users);

            return users;
        }

        public async Task<User?> Insert(User insertObject)
        {
            await Model.AddAsync(insertObject);

            return insertObject;
        }

        public async Task<IEnumerable<User>> Insert(IEnumerable<User> insertObjects)
        {
            await Model.AddRangeAsync(insertObjects);
            return insertObjects;
        }

        public async Task<User?> Update(User_UpdateDto dto)
        {
            byte[]? salt = null;
            User user = new User
            {
                Id = dto.Id,
                Username = dto.Username,
                Password = dto.Password != null ? _encryptionUtils.HashPassword(dto.Password, out salt) : null,
                Salt = salt,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            _context.Entry<User>(user).State = EntityState.Modified;

            return user;
        }

        public async Task<User?> Update(User updateObject)
        {
            _context.Entry<User>(updateObject).State = EntityState.Modified;
            return updateObject;
        }

        public Task<User?> Delete(User_DeleteDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> Delete(User deleteObject)
        {
            _context.Entry<User>(deleteObject).State = EntityState.Deleted;
            return deleteObject;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var users = (await Get())
                .AsQueryable()
                .Where(x => x.Username == username)
                .Include(x => x.UserKey)
                .Include(x => x.UserRoles)
                .ThenInclude(r => r.Role);
                

            return users.FirstOrDefault();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var users = (await Get())
                .AsQueryable()
                .Where(x => Guid.Equals(x.Id, id))
                .Include(x => x.UserKey)
                .Include(x => x.UserRoles)
                .ThenInclude(r => r.Role);
            return users.FirstOrDefault();
        }
    }
}
