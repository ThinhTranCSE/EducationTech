using EducationTech.Business.Business;
using EducationTech.Business.Business.Interfaces;
using EducationTech.DataAccess.Business.Interfaces;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.DataAccess.Master.Interfaces;
using EducationTech.Shared.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace EducationTech.Testing.Units.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private AuthService _authService;
        private Mock<EducationTechContext> _mainDbContextMock;
        private Mock<IAuthUtils> _authUtils;
        private Mock<IEncryptionUtils> _encryptionUtils;
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserKeyRepository> _userKeyRepository;
        private Mock<IUserRoleRepository> _userRoleRepository;
        private Mock<ICacheService> _cacheService;

        [SetUp]
        public void Setup()
        {
            var dbCxtTransactionMock = new Mock<IDbContextTransaction>();
            dbCxtTransactionMock.Setup(x => x.CommitAsync(CancellationToken.None)).Returns(Task.CompletedTask);
            var dbFacadeMock = new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            dbFacadeMock.Setup(x => x.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbCxtTransactionMock.Object);
            _mainDbContextMock = new Mock<EducationTechContext>(
                new DbContextOptionsBuilder<EducationTechContext>().Options,
                new Mock<IConfiguration>().Object
                );
            _mainDbContextMock.Setup(x => x.Database).Returns(dbFacadeMock.Object);

            _authUtils = new Mock<IAuthUtils>();
            _encryptionUtils = new Mock<IEncryptionUtils>();
            _userRepository = new Mock<IUserRepository>();
            _userKeyRepository = new Mock<IUserKeyRepository>();
            _userRoleRepository = new Mock<IUserRoleRepository>();
            _cacheService = new Mock<ICacheService>();
            //_authService = new AuthService(_mainDbContextMock.Object, _authUtils.Object, _encryptionUtils.Object, _userRepository.Object, _userKeyRepository.Object, _userRoleRepository.Object, _cacheService.Object);

        }

        //[Test]
        //public void Login_ShouldReturnTokensResponse_WhenUserExistedAndCorrectPassword()
        //{
        //    // Arrange
        //    User mockedUser = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = "mockedUsername",
        //        Password = "mockedPasswordHashed",
        //    };

        //    _userRepository.Setup(x => x.GetUserByUsername("mockedUsername")).ReturnsAsync(() => mockedUser);
        //    _encryptionUtils.Setup(x => x.VerifyPassword("mockedPassword", "mockedPasswordHashed", It.IsAny<byte[]>()))
        //        .Returns(true);
        //    _authUtils.Setup(x => x.GenerateToken(It.IsAny<Claim[]>(), It.IsAny<SecurityKey>(), It.IsAny<bool>()))
        //        .Returns("mockedJwtToken");


        //    // Act
        //    var result = _authService.Login(new LoginDto
        //    {
        //        Username = "mockedUsername",
        //        Password = "mockedPassword"
        //    }).GetAwaiter().GetResult();

        //    // Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(result.AccessToken, Is.Not.Null);
        //        Assert.That(result.RefreshToken, Is.Not.Null);
        //    });
        //}

        //[Test]
        //public void Login_ShouldThrowHttpException_WhenUserNotExisted()
        //{
        //    // Arrange
        //    User mockedUser = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = "mockedUsername",
        //        Password = "mockedPasswordHashed",
        //    };

        //    _userRepository.Setup(x => x.GetUserByUsername("mockedUsername")).ReturnsAsync(() => mockedUser);
        //    _encryptionUtils.Setup(x => x.VerifyPassword("mockedPassword", "mockedPasswordHashed", It.IsAny<byte[]>()))
        //        .Returns(false);

        //    // Act
        //    var exception = Assert.Throws<HttpException>(() => _authService.Login(new LoginDto
        //    {
        //        Username = "notExistedUser",
        //        Password = "mockedPassword"
        //    }).GetAwaiter().GetResult());

        //    // Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(exception.StatusCode, Is.EqualTo(401));
        //        Assert.That(exception.Message, Is.EqualTo("Username do not exist"));
        //    });
        //}

        //[Test]
        //public void Login_ShouldThrowHttpException_WhenPasswordIsWrong()
        //{
        //    // Arrange
        //    User mockedUser = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        Username = "mockedUsername",
        //        Password = "mockedPasswordHashed",
        //    };

        //    _userRepository.Setup(x => x.GetUserByUsername("mockedUsername")).ReturnsAsync(() => mockedUser);
        //    _encryptionUtils.Setup(x => x.VerifyPassword("mockedPassword", "mockedPasswordHashed", It.IsAny<byte[]>()))
        //        .Returns(false);

        //    // Act
        //    var exception = Assert.Throws<HttpException>(() => _authService.Login(new LoginDto
        //    {
        //        Username = "mockedUsername",
        //        Password = "wrongPassword"
        //    }).GetAwaiter().GetResult());


        //    // Assert
        //    Assert.Multiple(() =>
        //    {
        //        Assert.That(exception.StatusCode, Is.EqualTo(401));
        //        Assert.That(exception.Message, Is.EqualTo("Wrong password"));
        //    });
        //}


    }
}
