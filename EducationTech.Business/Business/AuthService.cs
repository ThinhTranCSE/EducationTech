using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Auth;
using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.DataAccess.Abstract;
using EducationTech.DataAccess.Entities.Business;
using EducationTech.DataAccess.Entities.Master;
using EducationTech.DataAccess.Entities.Recommendation;
using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EducationTech.Business.Business
{
    public class AuthService : IAuthService
    {
        private readonly IEncryptionUtils _encryptionUtils;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthUtils _authUtils;
        private readonly ICacheService _cacheService;

        public AuthService(
            IAuthUtils authUtils,
            IEncryptionUtils encryptionUtils,
            IUnitOfWork unitOfWork,
            ICacheService cacheService
            )
        {
            _authUtils = authUtils;

            _encryptionUtils = encryptionUtils;

            _cacheService = cacheService;

            _unitOfWork = unitOfWork;
        }


        public async Task<TokensReponseDto> Login(LoginDto loginDto)
        {
            var userQuery = _unitOfWork.Users.Find(u => u.Username == loginDto.Username);
            userQuery = userQuery
                .Include(u => u.UserKey)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .Include(u => u.Learner);

            User? user = await userQuery.FirstOrDefaultAsync();



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
            .Concat(
                user.UserRoles.Select(us => us.Role).SelectMany(r => r.RolePermissions).Select(rp => new Claim("permissions", rp.Permission.Name))
            )
            .ToArray();

            var refreshClaims = new Claim[]
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("refresh", "true")
            };

            using var rsa = RSA.Create();

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                if (user.UserKey == null)
                {
                    var userKey = new UserKey()
                    {
                        UserId = user.Id,
                        PublicKey = rsa.ToXmlString(false)
                    };

                    _unitOfWork.UserKeys.Add(userKey);
                    _unitOfWork.SaveChanges();

                    user.UserKey = userKey;
                }
                else
                {
                    user.UserKey.PublicKey = rsa.ToXmlString(false);

                }
                _unitOfWork.SaveChanges();

                transaction.Commit();

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
            User? user = _unitOfWork.Users.Find(u => u.Id == userId)
                .Include(u => u.UserKey)
                .FirstOrDefault();
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "User not found");
            }

            //if user and key not null, then remove key from db and cache
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                if (user.UserKey == null)
                {
                    throw new HttpException(HttpStatusCode.NotFound, "User did not login");
                }
                _unitOfWork.UserKeys.Remove(user.UserKey);

                _unitOfWork.SaveChanges();
                transaction.Commit();

                await _cacheService.RemoveAsync($"UserKey_{user.Id}");
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<TokensReponseDto> RefreshExpiredTokens(Guid userId)
        {
            User? user = await _unitOfWork.Users.GetAll()
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

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                if (user.UserKey == null)
                {
                    var userKey = new UserKey()
                    {
                        UserId = user.Id,
                        PublicKey = rsa.ToXmlString(false)
                    };

                    _unitOfWork.UserKeys.Add(userKey);
                    _unitOfWork.SaveChanges();

                    user.UserKey = userKey;
                }
                else
                {
                    user.UserKey.PublicKey = rsa.ToXmlString(false);

                }
                _unitOfWork.SaveChanges();

                transaction.Commit();

                await _cacheService.SetAsync($"UserKey_{user.Id}", user.UserKey.PublicKey, TimeSpan.FromMinutes(10));
            }
            catch (Exception e)
            {
                transaction.Rollback();
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
            User? user = _unitOfWork.Users.Find(u => u.Username == registerDto.Username).FirstOrDefault();
            if (user != null)
            {
                throw new HttpException(HttpStatusCode.Conflict, "Username already exists");
            }
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                string hashedPassword = _encryptionUtils.HashPassword(registerDto.Password, out var salt);

                User createdUser = new User()
                {
                    Username = registerDto.Username,
                    Password = hashedPassword,

                    Email = registerDto.Email,

                    Salt = salt

                };

                _unitOfWork.Users.Add(createdUser);
                _unitOfWork.SaveChanges();

                var roles = _unitOfWork.Roles.Find(r => registerDto.RoleIds.Contains(r.Id)).ToList();

                var createdUserRoles = roles.Select(r =>
                {
                    return new UserRole()
                    {
                        UserId = createdUser.Id,
                        RoleId = r.Id
                    };
                });

                _unitOfWork.UserRoles.AddRange(createdUserRoles);
                _unitOfWork.SaveChanges();

                if (roles.Any(r => r.Name == "Learner"))
                {
                    if (registerDto.SpecialityId == null)
                    {
                        throw new HttpException(HttpStatusCode.BadRequest, "SpecialityId is required with learner role");
                    }
                    var speciality = _unitOfWork.Specialities.Find(s => s.Id == registerDto.SpecialityId).FirstOrDefault();
                    if (speciality == null)
                    {
                        throw new HttpException(HttpStatusCode.NotFound, "Speciality not found");
                    }

                    var learnerCount = _unitOfWork.Learners.GetAll().Count();
                    var learner = new Learner()
                    {
                        UserId = createdUser.Id,
                        SpecialityId = speciality.Id,
                        IdentificationNumber = $"201{(learnerCount + 1):0000}"
                    };

                    _unitOfWork.Learners.Add(learner);
                    _unitOfWork.SaveChanges();
                }


                transaction.Commit();
                return createdUser;
            }
            catch (Exception e)
            {
                transaction.Rollback();
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
                User user = _unitOfWork.Users.GetAll().Include(u => u.UserKey).FirstOrDefault(u => u.Id == userId);
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
            var exp = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (exp == null)
            {
                return null;
            }
            if (long.Parse(exp) < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return null;
            }
            return userId != null ? Guid.Parse(userId) : null;
        }


        public User? GetUserFromToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            Guid? userId = GetUserIdFromToken(token.Split(" ")[1]);
            return _unitOfWork.Users.GetAll()
                .Where(u => u.Id == userId)
                .Include(u => u.Learner)
                .AsNoTracking()
                .FirstOrDefault();
        }
    }
}
