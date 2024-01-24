using EducationTech.Business.DTOs.Business.Auth;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Services.Abstract;

namespace EducationTech.Business.Services.Business.Interfaces
{
    public interface IAuthService : IService
    {
        Task<TokensReponseDto> Login(LoginDto loginDto);
        Task<User?> Register(RegisterDto registerDto);

        Task<bool?> Logout(Guid userId);

        Task<TokensReponseDto> RefreshExpiredTokens(Guid userId);


    }
}
