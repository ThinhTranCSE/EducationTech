using EducationTech.Business.DTOs.Business.Auth;
using EducationTech.Business.DTOs.Masters.User;
using EducationTech.Business.Models.Business;
using EducationTech.Business.Models.Master;
using EducationTech.Business.Repositories.Business.Interfaces;
using EducationTech.Business.Repositories.Master.Interfaces;
using EducationTech.Business.Services.Business;
using EducationTech.Business.Services.Business.Interfaces;
using EducationTech.Databases;
using EducationTech.Utilities;
using EducationTech.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EducationTech.Testing.Units.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private AuthService _authService; 
        private Mock<MainDatabaseContext> _mainDbContextMock;
        private Mock<IAuthUtils> _authUtils;
        private Mock<IEncryptionUtils> _encryptionUtils;
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserKeyRepository> _userKeyRepository;
        private Mock<ICacheService> _cacheService;

        [SetUp]
        public void Setup()
        {
            var dbCxtTransactionMock = new Mock<IDbContextTransaction>();
            dbCxtTransactionMock.Setup(x => x.CommitAsync(CancellationToken.None)).Returns(Task.CompletedTask);
            var dbFacadeMock = new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            dbFacadeMock.Setup(x => x.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbCxtTransactionMock.Object);
            _mainDbContextMock = new Mock<MainDatabaseContext>(
                new DbContextOptionsBuilder<MainDatabaseContext>().Options,
                new Mock<IConfiguration>().Object
                );
            _mainDbContextMock.Setup(x => x.Database).Returns(dbFacadeMock.Object);

            _authUtils = new Mock<IAuthUtils>();
            _encryptionUtils = new Mock<IEncryptionUtils>();
            _userRepository = new Mock<IUserRepository>();
            _userKeyRepository = new Mock<IUserKeyRepository>();
            _cacheService = new Mock<ICacheService>();
            _authService = new AuthService(_mainDbContextMock.Object, _authUtils.Object, _encryptionUtils.Object, _userRepository.Object, _userKeyRepository.Object, _cacheService.Object);

        }

        [Test]
        public void Login_ShouldReturnTokensResponse_WhenUserExistedAndCorrectPassword()
        {
            // Arrange
            User mockedUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "mockedUsername",
                Password = "mockedPasswordHashed",
            };

            _userRepository.Setup(x => x.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(() => mockedUser);
            _encryptionUtils.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns(true);
            _authUtils.Setup(x => x.GenerateToken(It.IsAny<Claim[]>(), It.IsAny<SecurityKey>(), It.IsAny<bool>()))
                .Returns("mockedJwtToken");


            // Act
            var result = _authService.Login(new LoginDto
            {
                Username = "mockedUsername",
                Password = "mockedPassword"
            }).GetAwaiter().GetResult();

            // Assert
            Assert.That(result.AccessToken, Is.Not.Null);
            Assert.That(result.RefreshToken, Is.Not.Null);
        }
    }
}
