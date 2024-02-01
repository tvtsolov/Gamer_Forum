using Forum_System.Exceptions;
using Forum_System.Repositories.Contracts;
using Forum_System.Services;
using Moq;
using Thread = Forum_System.Models.Thread;


namespace Forum_System.Tests.Services.Thread_Service
{
    [TestClass]
    public class Create_Should
    {
        [TestMethod]
        public void ReturnCreatedThread_When_ParamsAreValid()
        {

            // Arrange
            var testThread = TestHelper.GetTestThread();
            var testUser = TestHelper.GetTestUser();

            var repositoryMock = new Mock<IThreadRepository>();
            var userServiceMock = new Mock<IUserService>();

            repositoryMock
                .Setup(repo => repo.CheckDuplicateTitle(It.IsAny<string>()))
                .Returns(false);
            repositoryMock
                .Setup(repo => repo.Create(It.IsAny<Thread>()))
                .Returns(testThread);

            var sut = new ThreadService(repositoryMock.Object, userServiceMock.Object);

            // Act

            var createdThread = sut.Create(testUser.Id, testThread);

            // Assert

            Assert.IsTrue(testThread.Equals(createdThread));

        }


        [TestMethod]
        public void ThrowException_When_ThreadTitleExsists()
        {
            // Arrange
            var testThread = TestHelper.GetTestThread();
            var testUser = TestHelper.GetTestUser();

            var repositoryMock = new Mock<IThreadRepository>();
            var userServiceMock = new Mock<IUserService>();

            repositoryMock
                .Setup(repo => repo.Create(It.IsAny<Thread>()))
                .Returns(testThread);

            repositoryMock
                .Setup(repo => repo.CheckDuplicateTitle(It.IsAny<string>()))
                .Returns(true);

            var sut = new ThreadService(repositoryMock.Object, userServiceMock.Object);

            // Act & Assert

            Assert.ThrowsException<DuplicateEntityException>(() => sut.Create(testUser.Id, testThread), $"Thread with Title \"{testThread.Title}\" already exists");
        }
    }
}
