using AutoMapper;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Users;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EducationTech.Business.Master
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<UserDto?> GetUserById(Guid id)
        {
            var user = _unitOfWork.Users.Find(u => u.Id == id)
                .Include(u => u.Roles)
                .FirstOrDefault();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<User?> UpdateUser(Guid userId, User_UpdateDto updateDto, User currentUser)
        {

            try
            {
                var user = _unitOfWork.Users.Find(u => u.Id == userId)
                    .Include(u => u.Roles)
                    .Include(u => u.UserRoles)
                    .FirstOrDefault();

                if (user == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User not found");
                }
                _unitOfWork.Entry(currentUser)
                    .Collection(u => u.Roles)
                    .Load();

                var adminRole = _unitOfWork.Roles.Find(r => r.Name == "Admin").FirstOrDefault();
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

                    _unitOfWork.UserRoles.AddRange(roleAddeds);
                    _unitOfWork.UserRoles.RemoveRange(roleRemoveds);

                }
                _unitOfWork.SaveChanges();

                return user;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
