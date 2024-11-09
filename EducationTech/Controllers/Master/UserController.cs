using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Master.Interfaces;
using EducationTech.Business.Shared.DTOs.Masters.Users;
using EducationTech.Controllers.Abstract;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationTech.Controllers.Master;

[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;
    public UserController(IUserService userService, EducationTechContext context, IAuthService authService, ISessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<List<UserDto>> GetAll()
    {
        return await _userService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<UserDto?> GetUserById(Guid id)
    {
        return await _userService.GetUserById(id);
    }

    [HttpGet("me")]
    public async Task<UserDto?> GetCurrentUser()
    {

        return await _userService.GetUserById(_sessionService.CurrentUser!.Id);
    }

    [HttpPatch("{id}")]
    public async Task<User?> UpdateUser(Guid id, [FromBody] User_UpdateDto updateDto)
    {
        return await _userService.UpdateUser(id, updateDto);
    }
}
