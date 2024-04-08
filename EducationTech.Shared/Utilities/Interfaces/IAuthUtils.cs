using EducationTech.Shared.Utilities.Abstract;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EducationTech.Shared.Utilities.Interfaces
{
    public interface IAuthUtils : IUtils
    {
        string GenerateToken(IEnumerable<Claim> claims, SecurityKey privateKey, bool isRefresh = false);
    }
}
