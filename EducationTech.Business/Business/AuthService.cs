using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Net;
using Microsoft.EntityFrameworkCore;
using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Auth;
using EducationTech.Shared.Utilities.Interfaces;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Entities.Business;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace EducationTech.Business.Business
{
    public class AuthService : IAuthService
    {
        private readonly IEncryptionUtils _encryptionUtils;
        private readonly IUserRepository _userRepository;
        private readonly IUserKeyRepository _userKeyRepository;
        private readonly IAuthUtils _authUtils;
        private readonly ICacheService _cacheService;
        private readonly EducationTechContext _context;

        public AuthService(
            EducationTechContext context,
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
                    var userKey = await _userKeyRepository.Insert(new UserKey()
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

                await _cacheService.SetAsync($"UserKey_{user.Id}", user.UserKey?.PublicKey, TimeSpan.FromMinutes(10));
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }



            var accessToken = _authUtils.GenerateToken(accessClaims, new RsaSecurityKey(rsa));
            var refreshToken = _authUtils.GenerateToken(refreshClaims, new RsaSecurityKey(rsa), true);

            return new TokensReponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<bool?> Logout(Guid userId)
        {
            User? user = (await _userRepository.Get(u => u.Id == userId))
                .Include(u => u.UserKey)
                .FirstOrDefault();
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not found");
            }

            //if user and key not null, then remove key from db and cache
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (user.UserKey == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User did not login");
                }
                await _userKeyRepository.Delete(user.UserKey);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                await _cacheService.RemoveAsync($"UserKey_{user.Id}");
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<TokensReponseDto> RefreshExpiredTokens(Guid userId)
        {
            User? user = await _userRepository.Model
                .Include(u => u.UserKey)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not found");
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
                    var userKey = await _userKeyRepository.Insert(new UserKey()
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
                throw;
            }



            var accessToken = _authUtils.GenerateToken(accessClaims, new RsaSecurityKey(rsa));
            var refreshToken = _authUtils.GenerateToken(refreshClaims, new RsaSecurityKey(rsa), true);

            return new TokensReponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<User?> Register(RegisterDto registerDto)
        {
            User? user = await _userRepository.GetUserByUsername(registerDto.Username);
            if (user != null)
            {
                throw new HttpException(HttpStatusCode.Conflict, "Username already exists");
            }

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                string hashedPassword = _encryptionUtils.HashPassword(registerDto.Password, out var salt);

                User? createdUser = await _userRepository.Insert(new User()
                {
                    Username = registerDto.Username,
                    Password = hashedPassword,

                    PhoneNumber = registerDto.PhoneNumber,
                    Email = registerDto.Email,
                    DateOfBirth = registerDto.DateOfBirth,

                    Salt = salt

                });

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return createdUser;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public IEnumerable<SecurityKey> KeysResolver(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            Guid? userId = GetUserIdFromToken(token);
            if (userId == null)
            {
                yield break;
            }
            string cacheKey = $"UserKey_{userId}";

            string? publicKey = _cacheService.TryGetAndSetAsync<string>(cacheKey, async () =>
            {
                User user = _context.Users.Include(u => u.UserKey).FirstOrDefault(u => u.Id == userId);
                return user?.UserKey?.PublicKey;
            },
            TimeSpan.FromMinutes(10))
                .GetAwaiter()
                .GetResult();


            if (publicKey == null) yield break;

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

        
        public User? GetUserFromToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            Guid? userId = GetUserIdFromToken(token.Split(" ")[1]);
            return _context.Users
                .Where(u => u.Id == userId).FirstOrDefault();
        }
    }
}
