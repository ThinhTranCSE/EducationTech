using EducationTech.Business.Abstract;
using EducationTech.Business.Shared.DTOs.Business.Auth;
using EducationTech.DataAccess.Entities.Master;
using Microsoft.IdentityModel.Tokens;

namespace EducationTech.Business.Business.Interfaces
{
    public interface IAuthService : IService
    {
        Task<TokensReponseDto> Login(LoginDto loginDto);
        Task<User?> Register(RegisterDto registerDto);

        Task<bool?> Logout(Guid userId);

        Task<TokensReponseDto> RefreshExpiredTokens(Guid userId);

        IEnumerable<SecurityKey> KeysResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);
        public Guid? GetUserIdFromToken(string token);
        public User? GetUserFromToken(string? token);
    }
}
