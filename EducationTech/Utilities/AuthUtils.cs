using Bogus.Extensions.UnitedKingdom;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Databases;
using EducationTech.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EducationTech.Utilities
{
    public class AuthUtils : IAuthUtils
    {
        private readonly IConfiguration _configuration;
        private readonly MainDatabaseContext _context;
        private readonly IUserRepository _userRepository;

        private readonly string _expiredAccessTime;
        private readonly string _expiredRefreshTime;
        public AuthUtils(IConfiguration configuration, MainDatabaseContext context, IUserRepository userRepository)
        {
            _configuration = configuration;
            _context = context;
            _userRepository = userRepository;

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

        public IEnumerable<SecurityKey> KeysResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            Guid? userId = GetUserIdFromToken(token);
            if (userId == null)
            {
                yield break;
            }
            User user = _context.Users.Include(u => u.UserKey).FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                yield break;
            }
            string publicKey = user.UserKey?.PublicKey;
            if (publicKey == null)
            {
                yield break;
            }
            var rsa = RSA.Create();
            rsa.FromXmlString(publicKey);

            var privateKey = new RsaSecurityKey(rsa);
            yield return privateKey;
        }

        public Guid? GetUserIdFromToken(string token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            return userId != null ? Guid.Parse(userId) : null;
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
