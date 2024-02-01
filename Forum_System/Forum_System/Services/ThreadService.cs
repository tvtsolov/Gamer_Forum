using Forum_System.Exceptions;
using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using System.Threading;

namespace Forum_System.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IThreadRepository threadRepository;
        private readonly IUserService userService;

        public ThreadService(IThreadRepository threadRepository, IUserService userService)
        {
            this.threadRepository = threadRepository;
            this.userService = userService;
        }

        public List<Thread> GetAll()
        {
            return threadRepository.GetAll();
        }

        public PaginatedList<Thread> FilterBy(ThreadQueryParameters parameters)
        {
            return threadRepository.FilterBy(parameters);
        }

        public List<Thread> FilterBy()
        {
            return threadRepository.FilterBy();
        }

        public Thread GetById(int id)
        {
            var thread = threadRepository.GetById(id);

            return thread;
        }
        public Thread GetByTitle(string title)
        {
            var thread = threadRepository.GetByTitle(title);
            return thread;
        }
        public Thread Create(int userId, Thread thread)
        {
            if (threadRepository.CheckDuplicateTitle(thread.Title))
                throw new DuplicateEntityException($"Thread with Title \"{thread.Title}\" already exists.");

            var user = userService.GetById(userId);
            userService.CheckUserBlockStatus(user);

            thread.AuthorId = userId;
            return threadRepository.Create(thread);
        }
        public Thread AddRating(int threadId, double rating, int userId)
        {
            return threadRepository.AddRating(threadId, rating, userId);
        }

        public bool TryRemoveRating(int ratingId, int threadId)
        {
            return threadRepository.TryRemoveRating(ratingId, threadId);
        }

        public bool Delete(int loggedId, int id)
        {
            var loggedUser = userService.GetById(loggedId);
            var threadToDelete = threadRepository.GetById(id);

            CheckUserAuthorization(loggedUser, threadToDelete);

            return threadRepository.Delete(id);
        }
        public Thread Update(int userId, int threadId, string updatedContent, string updatedTitle)
        {
            var user = userService.GetById(userId);
            userService.CheckUserBlockStatus(user);

            return threadRepository.Update(userId, threadId, updatedContent, updatedTitle);
        }

        public void CheckUserAuthorization(User loggedUser, Thread thread)
        {
            if (!loggedUser.IsAdmin && loggedUser.Id != thread.AuthorId)
            {
                throw new UnauthorizedAccessException("You are not the author of this thread!");
            }
        }


    }
}
