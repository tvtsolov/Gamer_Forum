using Forum_System.Repositories.Contracts;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Moq;
using User = Forum_System.Models.User;

namespace Forum_System.Tests.Services.User_Service
{
    [TestClass]
    public class GetAll_Should
    {
        [TestMethod]
        public void ReturnUsers_When_UsersExist()
        {
            // Arrange
            var testUser = TestHelper.GetTestUser();
            List<User> users = new List<User> { testUser };

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(users);
            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act
            var expectedUsers = sut.GetAll();

            // Assert
            Assert.IsTrue(expectedUsers.Count > 0);
        }

        [TestMethod]
        public void ReturnEmptyList_When_NoUsersExist()
        {
            // Arrange
            List<User> users = new List<User>();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(users);

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act
            IList<User> expectedUsers = sut.GetAll();

            // Assert
            Assert.IsTrue(expectedUsers.Count == 0);

        }
    }
}
