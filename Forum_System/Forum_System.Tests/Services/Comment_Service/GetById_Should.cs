using Forum_System.Helpers;
using Forum_System.Repositories.Contracts;
using Forum_System.Services.Contracts;
using Forum_System.Services;
using Moq;
using Forum_System.Exceptions;
using Forum_System.Models;

namespace Forum_System.Tests.Services.Comment_Service
{
    [TestClass]
    public class GetById_Should
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
        public void ReturnCorrectComment_When_IdIsValid()
        {
            // Arrange
            Comment expectedComment = TestHelper.GetTestComment();

            repositoryMock.Setup(repo => repo.GetById(1)).Returns(expectedComment);

            var sut = new CommentService(repositoryMock.Object, userServiceMock.Object, authServiceMock.Object, mapperMock.Object);

            // Act
            Comment actualComment = sut.GetById(1);

            // Assert
            Assert.IsTrue(expectedComment.Equals(actualComment));
        }

        [TestMethod]
        public void ThrowException_When_UserNotFound()
        {
            repositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Throws(new EntityNotFoundException($"Comment not found!"));

            var sut = new CommentService(repositoryMock.Object, userServiceMock.Object, authServiceMock.Object, mapperMock.Object);

            Assert.ThrowsException<EntityNotFoundException>(() => sut.GetById(1));
        }
    }
}
