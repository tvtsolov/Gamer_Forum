using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum_System.Exceptions;
using Forum_System.Repositories.Contracts;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Moq;
using User = Forum_System.Models.User;

namespace Forum_System.Tests.Services.User_Service
{
    [TestClass]
    public class GetByName_Should
    {
        [TestMethod]
        public void ReturnCorrectUser_When_NameIsValid()
        {
            // Arrange
            User expectedUser = TestHelper.GetTestUser();

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock.Setup(repo => repo.GetByName("test user")).Returns(expectedUser);

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            // Act
            User actualUser = sut.GetByName("test user");

            // Assert
            Assert.IsTrue(expectedUser.Equals(actualUser));
        }

        [TestMethod]
        public void ThrowException_When_UserNotFound()
        {
            User expectedUser = null;

            var repositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            repositoryMock
                .Setup(repo => repo.GetByName("name"))
                .Throws(new EntityNotFoundException($"User not found!"));

            var sut = new UserService(repositoryMock.Object, authServiceMock.Object);

            Assert.ThrowsException<EntityNotFoundException>(() => sut.GetByName("name"));
        }
    }
}
