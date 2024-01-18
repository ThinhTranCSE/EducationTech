using Bogus.Extensions.UnitedKingdom;
using EducationTech.Databases;
using EducationTech.Models.Master;
using EducationTech.Repositories.Master.Interfaces;
using EducationTech.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EducationTech.Utilities
{
    public class AuthUltils : IAuthUltils
    {
        private readonly IConfiguration _configuration;
        private readonly MainDatabaseContext _context;
        private readonly IUserRepository _userRepository;

        public AuthUltils(IConfiguration configuration, MainDatabaseContext context, IUserRepository userRepository)
        {
            _configuration = configuration;
            _context = context;
            _userRepository = userRepository;
        }
       
        public string GenerateToken(IEnumerable<Claim> claims, SecurityKey privateKey)
        {
            var jwtTokenHanlder = new JwtSecurityTokenHandler();
            
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
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
                return Array.Empty<SecurityKey>();
            }
            User user = _context.Users.Include(u => u.UserKey).FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return Array.Empty<SecurityKey>();
            }
            string publicKey = user.UserKey?.PublicKey;
            if (publicKey == null)
            {
                return Array.Empty<SecurityKey>();
            }
            var rsa = RSA.Create();
            rsa.FromXmlString(publicKey);

            var privateKey = new RsaSecurityKey(rsa);
            return new List<SecurityKey>() { privateKey };
        }

        private Guid? GetUserIdFromToken(string token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            return userId != null ? Guid.Parse(userId) : null;
        }
    }
}
