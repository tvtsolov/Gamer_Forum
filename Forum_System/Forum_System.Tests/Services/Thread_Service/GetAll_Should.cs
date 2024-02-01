using Forum_System.Repositories.Contracts;
using Forum_System.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thread = Forum_System.Models.Thread;

namespace Forum_System.Tests.Services.Thread_Service
{
    [TestClass]
    public class GetAll_Should
    {
        [TestMethod]
        public void ReturnThreads_When_ThreadsExist()
        {

        }

        [TestMethod]
        public void ReturnEmptyList_When_NoThreadsExist()
        {
            // Arrange
            List<Thread> threads = new List<Thread>();

            var repositoryMock = new Mock<IThreadRepository>();
            var userServiceMock = new Mock<IUserService>();

            repositoryMock
                .Setup(repo => repo.GetAll())
                .Returns(threads);

            var sut = new ThreadService(repositoryMock.Object, userServiceMock.Object);

            // Act
            List<Thread> expectedThreads = sut.GetAll();

            // Assert
            Assert.IsTrue(expectedThreads.Count == 0);

        }  
    }
}
