using Forum_System.Models;
using Forum_System.Repositories.Contracts;
using Forum_System.Services.Contracts;
using Forum_System.Services;
using Moq;
using Forum_System.Repositories;
using Forum_System.Helpers;


namespace Forum_System.Tests.Services.Comment_Service
{
    [TestClass]
    public class GetAll_Should
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
        public void ReturnComments_When_CommentsExist()
        {
            // Arrange
            var testComment = TestHelper.GetTestComment();
            List<Comment> comments = new List<Comment> { testComment };

            repositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(comments);

            var sut = new CommentService(repositoryMock.Object, userServiceMock.Object, authServiceMock.Object, mapperMock.Object);

            // Act
            var expectedComments = sut.GetAll();

            // Assert
            Assert.IsTrue(expectedComments.Count > 0);
        }

        [TestMethod]
        public void ReturnEmptyList_When_NoCommentsExist()
        {
            // Arrange
            List<Comment> comments = new List<Comment>();

            repositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(comments);

            var sut = new CommentService(repositoryMock.Object, userServiceMock.Object, authServiceMock.Object, mapperMock.Object);

            // Act
            IList<Comment> expectedComments = sut.GetAll();

            // Assert
            Assert.IsTrue(expectedComments.Count == 0);

        }
    }
}
