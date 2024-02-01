using Forum_System.Services;
using Forum_System.Services.Contracts;
using Forum_System.Repositories.Contracts;
using Moq;
using User = Forum_System.Models.User;
using Forum_System.Exceptions;

namespace Forum_System.Tests.Services.User_Service
{
    [TestClass]
    public class Update_Should
    {
        [TestMethod]
        public void UpdateUser_When_LoggedIdEqualsIdToUpdate()
        {
            // Arrange
            var loggedUser = TestHelper.GetTestUser();
            var updatedUser = TestHelper.GetUpdateData();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(loggedUser); 

            repositoryMock
                .Setup(repo => repo.Update(loggedUser.Id, updatedUser))
                .Returns(updatedUser); 

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act
            User result = sut.Update(loggedUser.Id, loggedUser.Id, updatedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUser.Id, result.Id);
        }

        [TestMethod]
        public void ThrowException_When_LoggedIdNotEqualIdToUpdate()
        {
            // Arrange
            var loggedUser = TestHelper.GetTestUser();
            var idToUpdate = 123;
            var updatedUser = TestHelper.GetUpdateData();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(loggedUser);

            authServiceMock
                .Setup(authService => authService.CheckUserAuthorization(loggedUser, It.IsAny<User>()))
                .Throws<UnauthorizedAccessException>();

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act and Assert
            Assert.ThrowsException<UnauthorizedAccessException>(() => sut.Update(loggedUser.Id, idToUpdate, updatedUser));
        }

        [TestMethod]
        public void UpdateNameAndPhone_When_UserIsAdmin()
        {
            // Arrange
            var adminUser = TestHelper.GetTestAdmin();
            var updatedUser = TestHelper.GetAdminUpdateData();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(adminUser);

            authServiceMock
                .Setup(authService => authService.CheckAdminAuthorization(adminUser));

            repositoryMock
                .Setup(repo => repo.Update(adminUser.Id, updatedUser))
                .Returns(updatedUser);

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act
            User result = sut.Update(adminUser.Id, adminUser.Id, updatedUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUser.Username, result.Username);
            Assert.AreEqual(updatedUser.PhoneNumber, result.PhoneNumber);
        }

        [TestMethod]
        public void ThrowException_When_NonAdminChangesNameAndPhone()
        {
            // Arrange
            var nonAdminUser = TestHelper.GetTestUser();
            var updatedUser = TestHelper.GetAdminUpdateData();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetById(nonAdminUser.Id))
                .Returns(TestHelper.GetTestUser());

            authServiceMock
                .Setup(authService => authService.CheckAdminAuthorization(nonAdminUser))
                .Throws<UnauthorizedAccessException>();

            repositoryMock
                .Setup(repo => repo.Update(nonAdminUser.Id, updatedUser))
                .Throws<UnauthorizedAccessException>();

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act and Assert
            Assert.ThrowsException<UnauthorizedAccessException>(() => sut.Update(nonAdminUser.Id, nonAdminUser.Id, updatedUser));
        }
    }
}
