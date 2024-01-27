using EducationTech.Business.DTOs.Business.Auth;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Databases;
using EducationTech.Business.DTOs.Business.UserKey;
using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Models.Business;
using EducationTech.Business.Services.Abstract;
using EducationTech.Utilities.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Security.Cryptography;
using EducationTech.Exceptions.Http;
using System.Net;

namespace EducationTech.Business.Services.Business
{
    public class AuthService : IAuthService
    {
        private readonly IEncryptionUtils _encryptionUtils;
        private readonly IUserRepository _userRepository;
        private readonly IUserKeyRepository _userKeyRepository;
        private readonly IAuthUtils _authUtils;
        private readonly MainDatabaseContext _context;
        private readonly ICacheService _cacheService;   
        
        public AuthService(
            MainDatabaseContext context,
            IAuthUtils authUtils,
            IEncryptionUtils encryptionUtils,
            IUserRepository userRepository,
            IUserKeyRepository userKeyRepository,
            ICacheService cacheService
            )
        {
            _context = context;
            _authUtils = authUtils;
            _userRepository = userRepository;
            _encryptionUtils = encryptionUtils;
            _userKeyRepository = userKeyRepository;
            _cacheService = cacheService;
        }

        public async Task<TokensReponseDto> Login(LoginDto loginDto)
        {
            User? user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Username do not exist");
            }
            if (!_encryptionUtils.VerifyPassword(loginDto.Password, user.Password, user.Salt))
            {
                throw new HttpException(HttpStatusCode.Unauthorized, "Wrong password");

            }

            var accessClaims = new Claim[]
            {
                new Claim("sub", user.Id.ToString()),
            }
            .Concat(
                user.UserRoles.Select(r => new Claim("roles", r.Role.Name))
             )
            .ToArray();

            var refreshClaims = new Claim[]
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

                await _cacheService.SetAsync($"UserKey_{user.Id}", user.UserKey.PublicKey, TimeSpan.FromMinutes(10));
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw e;
            }



            var accessToken = _authUtils.GenerateToken(accessClaims, new RsaSecurityKey(rsa));
            var refreshToken = _authUtils.GenerateToken(refreshClaims, new RsaSecurityKey(rsa), true);

            return new TokensReponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
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
            string hashedPassword = _encryptionUtils.HashPassword(registerDto.Password, out var salt);

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
