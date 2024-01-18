using EducationTech.DTOs.Business.Auth;
using EducationTech.Models.Master;
using EducationTech.Services.Abstract;

namespace EducationTech.Services.Business.Interfaces
{
    public interface IAuthService : IService
    {
        Task<TokensReponseDto> Login(LoginDto loginDto); 
        Task<User?> Register(RegisterDto registerDto);

        Task<bool?> Logout(Guid userId);

        Task<TokensReponseDto> RefreshExpiredTokens(Guid userId);


    }
}
