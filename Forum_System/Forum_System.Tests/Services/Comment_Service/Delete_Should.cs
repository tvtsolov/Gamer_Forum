using Forum_System.Helpers;
using Forum_System.Repositories.Contracts;
using Forum_System.Services.Contracts;
using Forum_System.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum_System.Exceptions;

namespace Forum_System.Tests.Services.Comment_Service
{
    [TestClass]
    public class Delete_Should
    {
        private Mock<ICommentRepository> repositoryMock;
        private Mock<IUserService> userServiceMock;
        private Mock<IAuthService> authServiceMock;
        private Mock<ModelMapper> mapperMock;

        [TestInitialize]
        public void Initialize()
        {
            repositoryMock = new Mock<ICommentRepository>();
            userServiceMock = new Mock<IUserService>();
            authServiceMock = new Mock<IAuthService>();
            mapperMock = new Mock<ModelMapper>();
        }

        [TestMethod]
        public void ReturnTrue_When_CommentDeleted()
        {
            // Arrange
            var loggedUserId = 1;
            var commentIdToDelete = 1; 
            var loggedUser = TestHelper.GetTestUser;

            userServiceMock.Setup(serv => serv.GetById(loggedUserId)).Returns(loggedUser);

            repositoryMock
                .Setup(repo => repo.GetById(commentIdToDelete))
                .Returns(TestHelper.GetTestComment()); 

            repositoryMock
                .Setup(repo => repo.Delete(commentIdToDelete))
                .Returns(true);

            var sut = new CommentService(repositoryMock.Object, userServiceMock.Object, authServiceMock.Object, mapperMock.Object);

            // Act
            var result = sut.Delete(loggedUserId, commentIdToDelete);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Delete_ThrowsUnauthorizedException_WhenUserNotAuthorized()
        {
            var loggedUserId = 999;
            var commentId = 1; 
            var loggedUser = TestHelper.GetTestInvalidUser; 

            userServiceMock
                .Setup(serv => serv.GetById(loggedUserId))
                .Returns(loggedUser);

            repositoryMock
                .Setup(repo => repo.GetById(commentId))
                .Returns(TestHelper.GetTestComment());

            var sut = new CommentService(repositoryMock.Object, userServiceMock.Object, authServiceMock.Object, mapperMock.Object);

            // Act
            Assert.ThrowsException<UnauthorizedAccessException>(() => sut.Delete(loggedUserId, commentId));
        }


    }
}
