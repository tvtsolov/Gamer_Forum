using Forum_System.Exceptions;
using Forum_System.Models;
using Forum_System.Repositories.Contracts;
using Forum_System.Services;
using Moq;
using Thread = Forum_System.Models.Thread;

namespace Forum_System.Tests.Services.Thread_Service
{
    [TestClass]
    public class GetById_Should
    {

        [TestMethod]
        public void ReturnCorrectThread_When_IdIsValid ()
        {
            // Arrange
            Thread expectedThread = TestHelper.GetTestThread();

            var repositoryMock = new Mock<IThreadRepository>();
            repositoryMock.Setup(repo => repo.GetById(1)).Returns(expectedThread);

            var userServiceMock = new Mock<IUserService>();

            var sut = new ThreadService(repositoryMock.Object, userServiceMock.Object);

            // Act
            Thread actualThread = sut.GetById(1);

            // Assert
            Assert.IsTrue(expectedThread.Equals(actualThread));
        }

        [TestMethod]
        public void ThrowException_When_ThreadNotFound()
        {
            var repositoryMock = new Mock<IThreadRepository>();
            var userServiceMock = new Mock<IUserService>();


            repositoryMock
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Throws(new EntityNotFoundException("Thread does not exist."));

            var sut = new ThreadService(repositoryMock.Object, userServiceMock.Object);

            Assert.ThrowsException<EntityNotFoundException>(() => sut.GetById(1));
        }

    }
}
