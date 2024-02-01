using Forum_System.Models;
using Forum_System.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Thread = Forum_System.Models.Thread;

namespace Forum_System.Tests
{
    public class TestHelper
    {
        public static Thread GetTestThread()
        {
            return new Thread
            {
                Id = 1,
                Title = "Test thread title",
                Content = "This is the test content of the thread",
                AuthorId = 1,  
                Date = DateTime.Now
            };
        }

        public static Comment GetTestComment()
        {
            return new Comment
            {
                Id = 1,
                Content = "This is the test content of the test comment.",
                ThreadId = 1,
                AuthorId = 1,
                CreationDate = DateTime.Now
            };
        }

        public static CommentUpdateDto GetTestUpdateCommentDto()
        {
            return new CommentUpdateDto()
            {
                Content = "This is the updated test comment content",
            };
        }

        public static CommentCreateDto GetTestCreateCommentDto()
        {
            return new CommentCreateDto
            {
                Content = "This is the test content of the test comment.",
                ThreadId = 1,
            };
        }

        public static List<Comment> GetTestCommentsList()
        {
            List<Comment> comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "test comemnt", AuthorId = 1 },
                new Comment { Id = 2, Content = "test comment number two", AuthorId = 3},
                new Comment { Id = 3, Content = "another test comment", AuthorId = 2}
            };
            return comments;
        }


        public static List<Thread> GetTestThreadList()
        {

            List<Thread> threads = new List<Thread>
            {
                new Thread
                {
                    Id = 1,
                    Title = "Test thread title",
                    Content = "This is the test content of the thread first",
                    AuthorId = 1,
                    Date = DateTime.Now.AddDays(-2).AddMinutes(2),
                    Comments = GetTestCommentsList()
                },
                new Thread
                {
                    Id = 2,
                    Title = "Second thread test title2",
                    Content = "This is the test content of the thread second",
                    AuthorId = 1,
                    Date = DateTime.Now.AddDays(-1).AddMinutes(32)
                },
                new Thread
                {
                    Id = 1,
                    Title = "Test thread title3",
                    Content = "This is the test content of the thread third",
                    AuthorId = 1,
                    Date = DateTime.Now.AddDays(-3).AddMinutes(234)
                },
                new Thread
                {
                    Id = 1,
                    Title = "Test thread title 4",
                    Content = "This is the test content of the thread forth",
                    AuthorId = 1,
                    Date = DateTime.Now.AddMinutes(-22)
                }
            };
            return threads;
        }

        public static User GetTestUser()
        {
            return new User
            {
                Id = 1,
                Username = "test user",
                Email = "useraddress@gmail.com",
                FirstName = "Johny",
                LastName = "Bravo",
                IsAdmin = false,
            };
        }

        public static User GetUpdateData()
        {
            return new User
            {
                FirstName = "Gosho",
                LastName = "Petrov"
            };
        }

        public static Comment GetCommentUpdateData()
        {
            return new Comment
            {
                Content = "This is the updated test comment content"
            };
        }

        public static User GetAdminUpdateData()
        {
            return new User
            {
                Username = "new username",
                PhoneNumber = "123456789"
            };
        }

        public static User GetTestInvalidUser()
        {
            return new User
            {
                Id = 999,
                Username = "invalid",
                Email = "otheraddress@gmail.com",
                FirstName = "Stevie",
                LastName = "Wonder",
                IsAdmin = false,
            };
        }

        public static User GetTestAdmin()
        {
            return new User
            {
                Id = 1,
                Username = "test admin",
                Email = "adminaddress@gmail.com",
                FirstName = "Addam",
                LastName = "Innel",
                IsAdmin = true,
            };
        }
    }
}
