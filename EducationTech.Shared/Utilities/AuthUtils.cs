using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EducationTech.Shared.Utilities
{
    public class AuthUtils : IAuthUtils
    {
        private readonly IConfiguration _configuration;

        private readonly string _expiredAccessTime;
        private readonly string _expiredRefreshTime;
        public AuthUtils(IConfiguration configuration)
        {
            _configuration = configuration;

            _expiredAccessTime = _configuration.GetValue<string>("JWT:ExpiredAccessTime");
            _expiredRefreshTime = _configuration.GetValue<string>("JWT:ExpiredRefreshTime");
        }

        public string GenerateToken(IEnumerable<Claim> claims, SecurityKey privateKey, bool isRefresh = false)
        {
            var jwtTokenHanlder = new JwtSecurityTokenHandler();

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = StringToDateTime(isRefresh ? _expiredRefreshTime : _expiredAccessTime),
                SigningCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256)
            };

            var token = jwtTokenHanlder.CreateToken(tokenDescription);
            return jwtTokenHanlder.WriteToken(token);
        }

        private DateTime? StringToDateTime(string value)
        {
            if (value == null || value.Length < 2)
            {
                return null;
            }
            string unit = value.Substring(value.Length - 1);
            switch (unit)
            {
                case "d":
                    return DateTime.UtcNow.AddDays(int.Parse(value.Substring(0, value.Length - 1)));
                case "h":
                    return DateTime.UtcNow.AddHours(int.Parse(value.Substring(0, value.Length - 1)));
                case "m":
                    return DateTime.UtcNow.AddMinutes(int.Parse(value.Substring(0, value.Length - 1)));
                case "s":
                    return DateTime.UtcNow.AddSeconds(int.Parse(value.Substring(0, value.Length - 1)));
            }

            return DateTime.Parse(value);
        }
    }
}
