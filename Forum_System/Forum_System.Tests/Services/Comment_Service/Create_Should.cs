using Forum_System.Helpers;
using Forum_System.Repositories.Contracts;
using Forum_System.Services.Contracts;
using Forum_System.Services;
using Moq;
using Forum_System.Models;

namespace Forum_System.Tests.Services.Comment_Service
{
    [TestClass]
    public class Create_Should
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
        public void Create_ReturnsCreatedComment_When_ParamsAreValid()
        {
            // Arrange
            var testDto = TestHelper.GetTestCreateCommentDto();
            var loggedUserId = 1;
            var testComment = TestHelper.GetTestComment();

            userServiceMock
                .Setup(us => us.GetById(loggedUserId))
                .Returns(TestHelper.GetTestUser()); // Assuming GetTestUser returns a valid user for testing purposes

            userServiceMock
                .Setup(us => us.CheckUserBlockStatus(It.IsAny<User>()))
                .Verifiable();

            repositoryMock
                .Setup(repo => repo.Create(It.IsAny<Comment>()))
                .Returns(testComment);

            var sut = new CommentService(repositoryMock.Object, userServiceMock.Object, authServiceMock.Object, mapperMock.Object);

            // Act
            var createdComment = sut.Create(loggedUserId, testDto);

            // Assert
            Assert.IsNotNull(createdComment);
            Assert.AreEqual(testComment, createdComment);
            userServiceMock.Verify();
        }
    }

}
