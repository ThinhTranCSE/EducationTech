using EducationTech.Databases;
using EducationTech.DTOs.Business.Auth;
using EducationTech.DTOs.Business.UserKey;
using EducationTech.DTOs.Masters.User;
using EducationTech.Models.Business;
using EducationTech.Models.Master;
using EducationTech.Repositories.Business.Interfaces;
using EducationTech.Repositories.Master.Interfaces;
using EducationTech.Services.Business.Interfaces;
using EducationTech.Utilities.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EducationTech.Services.Business
{
    public class AuthService : IAuthService
    {
        private readonly MainDatabaseContext _context;
        private readonly IAuthUltils _authUltils;
        private readonly IEncryptionUtils _encryptionUtils;
        private readonly IUserRepository _userRepository;
        private readonly IUserKeyRepository _userKeyRepository;
        public AuthService(
            MainDatabaseContext context,
            IAuthUltils authUltils,
            IEncryptionUtils encryptionUtils,
            IUserRepository userRepository,
            IUserKeyRepository userKeyRepository
            )
        {
            _context = context;
            _authUltils = authUltils;
            _userRepository = userRepository;
            _encryptionUtils = encryptionUtils;
            _userKeyRepository = userKeyRepository;
        }

        public async Task<TokensReponseDto> Login(LoginDto loginDto)
        {
            User? user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null)
            {
                return null;
            }
            if (!_encryptionUtils.VerifyPassword(loginDto.Password, user.Password, user.Salt))
            {
                return null;
            }

            Claim[] accessClaims = new Claim[]
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("roles", JsonConvert.SerializeObject(user.UserRoles.Select(r => r.Role.Name).ToList()))
            };

            Claim[] refreshClaims = new Claim[]
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("refresh", "true")
            };

            using var rsa = RSA.Create();

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (user.UserKey == null)
                {
                    var userKey = await _userKeyRepository.Insert(new UserKey_InsertDto()
                    {
                        UserId = user.Id,
                        PublicKey = rsa.ToXmlString(false)
                    });
                    user.UserKey = userKey;
                }
                else
                {
                    user.UserKey.PublicKey = rsa.ToXmlString(false);

                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw e;
            }



            var accessToken = _authUltils.GenerateToken(accessClaims, new RsaSecurityKey(rsa));
            var refreshToken = _authUltils.GenerateToken(refreshClaims, new RsaSecurityKey(rsa));

            return new TokensReponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public Task<bool?> Logout(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<TokensReponseDto> RefreshExpiredTokens(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> Register(RegisterDto registerDto)
        {
            byte[] salt;
            string hashedPassword = _encryptionUtils.HashPassword(registerDto.Password, out salt);

            User? createdUser = await _userRepository.Insert(new User_InsertDto()
            {
                Username = registerDto.Username,
                Password = hashedPassword,

                PhoneNumber = registerDto.PhoneNumber,
                Email = registerDto.Email,
                DateOfBirth = registerDto.DateOfBirth,

                Salt = salt

            });

            return createdUser;
        }
    }
}
