using Forum_System.Services;
using Forum_System.Services.Contracts;
using Forum_System.Repositories.Contracts;
using Moq;
using User = Forum_System.Models.User;
using Forum_System.Exceptions;

namespace Forum_System.Tests.Services.User_Service
{
    [TestClass]
    public class Delete_Should
    {
        [TestMethod]
        public void ReturnTrue_When_UserSuccessfullyDeleted()
        {
            // Arrange
            var testUser = TestHelper.GetTestUser();
            var loggedUser = TestHelper.GetTestAdmin();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(loggedUser);

            authServiceMock
                .Setup(authService => authService.CheckAdminAuthorization(loggedUser));

            repositoryMock
                .Setup(repo => repo.Delete(testUser.Id))
                .Returns(true);

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act
            bool isDeleted = sut.Delete(loggedUser.Id, testUser.Id);

            // Assert
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void ThrowException_When_UserNotAdmin()
        {
            // Arrange
            var testUser = TestHelper.GetTestUser();
            var loggedUser = TestHelper.GetTestInvalidUser();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(loggedUser);

            authServiceMock
                .Setup(authService => authService.CheckAdminAuthorization(loggedUser))
                .Throws<UnauthorizedAccessException>();

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act and Assert
            Assert.ThrowsException<UnauthorizedAccessException>(() => sut.Delete(loggedUser.Id, testUser.Id));
        }
    }
}
