using EducationTech.Utilities.Abstract;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EducationTech.Utilities.Interfaces
{
    public interface IAuthUltils : IUltils
    {
        string GenerateToken(IEnumerable<Claim> claims, SecurityKey privateKey);
        IEnumerable<SecurityKey> KeysResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);
    }
}
