using Microsoft.EntityFrameworkCore;
using System.Net;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.User;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Business.Shared.Exceptions.Http;

namespace EducationTech.Business.Master
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IUserRoleRepository _userRoleRepository;
        public UserService(ITransactionManager transactionFactory, IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _transactionManager = transactionFactory;
            _userRoleRepository = userRoleRepository;
        }


        public async Task<User?> GetUserById(Guid id)
        {
            var user = (await _userRepository.Get(u => u.Id == id))
                .Include(u => u.Roles)
                .FirstOrDefault();

            return user;
        }

        public async Task<User?> UpdateUser(Guid userId, User_UpdateDto updateDto, User currentUser)
        {

            try
            {
                var user = (await _userRepository.Get(u => u.Id == userId))
                    .Include(u => u.Roles)
                    .Include(u => u.UserRoles)
                    .FirstOrDefault();

                if (user == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User not found");
                }
                _userRepository.Entry(currentUser)
                    .Collection(u => u.Roles)
                    .Load();

                var adminRole = await _roleRepository.GetSingle(r => r.Name == "Admin");
                if (!currentUser.Roles.Any(r => r.Id == adminRole!.Id) && currentUser.Id != userId)
                {
                    throw new HttpException(HttpStatusCode.Unauthorized, "Not have permission to change information");
                }


                user.Map(updateDto);


                if (updateDto.RoleIds != null)
                {
                    var currentRoleIds = user.Roles.Select(r => r.Id).ToArray();

                    var roleAddedIds = updateDto.RoleIds.Except(currentRoleIds).ToArray();
                    var roleRemovedIds = currentRoleIds.Except(updateDto.RoleIds).ToArray();

                    IEnumerable<UserRole> roleAddeds = roleAddedIds.Select(roleId =>
                    {
                        return new UserRole
                        {
                            UserId = userId,
                            RoleId = roleId
                        };
                    });

                    IEnumerable<UserRole> roleRemoveds = user.UserRoles.Where(ur =>
                    {
                        return roleRemovedIds.Contains(ur.RoleId);
                    });

                    await Task.WhenAll(
                        _userRoleRepository.Insert(roleAddeds),
                        _userRoleRepository.Delete(roleRemoveds)
                    );

                }
                await _userRepository.Update(user);
                _transactionManager.SaveChanges();

                return user;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
