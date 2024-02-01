using Forum_System.Services;
using Forum_System.Services.Contracts;
using Forum_System.Repositories.Contracts;
using Moq;
using User = Forum_System.Models.User;
using Forum_System.Exceptions;

namespace Forum_System.Tests.Services.User_Service
{
    [TestClass]
    public class Create_Should
    {
        [TestMethod]
        public void ReturnCreatedUser_When_ParamsAreValid()
        {
            // Arrange
            var testUser = TestHelper.GetTestUser();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.CheckDuplicateEmail(It.IsAny<string>()))
                .Returns(false);
            repositoryMock
                .Setup(repo => repo.CheckDuplicateName(It.IsAny<string>()))
                .Returns(false);

            repositoryMock
                .Setup(repo => repo.Create(It.IsAny<User>()))
                .Returns(testUser);

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act
            var createdUser = sut.Create(testUser);

            // Assert
            Assert.IsTrue(testUser.Equals(createdUser));
        }

        [TestMethod]
        public void ThrowException_When_UsernameExsists()
        {
            // Arrange
            var testUser = TestHelper.GetTestUser();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.Create(It.IsAny<User>()))
                .Returns(testUser);

            repositoryMock
               .Setup(repo => repo.CheckDuplicateName(It.IsAny<string>()))
               .Returns(true);

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act & Assert
            Assert.ThrowsException<DuplicateEntityException>(() => sut.Create(testUser), $"Username {testUser.Username} is already in use!");
        }

        [TestMethod]
        public void ThrowException_When_EmailExsists()
        {
            // Arrange
            var testUser = TestHelper.GetTestUser();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.Create(It.IsAny<User>()))
                .Returns(testUser);

            repositoryMock
               .Setup(repo => repo.CheckDuplicateEmail(It.IsAny<string>()))
               .Returns(true);

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act & Assert
            Assert.ThrowsException<DuplicateEntityException>(() => sut.Create(testUser), $"Email {testUser.Email} is already in use!");
        }
    }
}
