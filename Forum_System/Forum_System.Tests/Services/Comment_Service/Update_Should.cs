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
using Forum_System.Models;

namespace Forum_System.Tests.Services.Comment_Service
{
    [TestClass]
    public class Update_Should
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

        /*
        [TestMethod]
        public void UpdateComment_When_LoggedIdEqualsAuthorId()
        {
            // Arrange
            var comment = TestHelper.GetTestComment();
            var loggedUser = TestHelper.GetTestUser();
            var updateData = TestHelper.GetTestUpdateCommentDto();
            var updatedComment = TestHelper.GetCommentUpdateData();

            userServiceMock
                .Setup(serv => serv.GetById(loggedUser.Id))
                .Returns(loggedUser);

            userServiceMock
                .Setup(serv => serv.CheckUserBlockStatus(loggedUser));


            repositoryMock
                .Setup(repo => repo.GetById(comment.Id))
                .Returns(TestHelper.GetTestComment());

            repositoryMock
                .Setup(repo => repo.Update(comment.Id, comment))
                .Returns(updatedComment);

            var sut = new CommentService(repositoryMock.Object, userServiceMock.Object, authServiceMock.Object, mapperMock.Object);

            // Act
            var result = sut.Update(loggedUser.Id, comment.Id, updateData);

            // Assert
            Assert.AreEqual(updateData.Content, result.Content);
        }
        */

    }
}
