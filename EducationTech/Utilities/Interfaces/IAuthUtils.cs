using EducationTech.Business.Models.Master;
using EducationTech.Utilities.Abstract;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EducationTech.Utilities.Interfaces
{
    public interface IAuthUtils : IUtils
    {
        string GenerateToken(IEnumerable<Claim> claims, SecurityKey privateKey, bool isRefresh = false);
        IEnumerable<SecurityKey> KeysResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);
        public Guid? GetUserIdFromToken(string token);
        public User? GetUserFromToken(string? token);

    }
}
