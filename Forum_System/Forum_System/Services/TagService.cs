using Forum_System.Models;
using Forum_System.Models.Contracts;
using Forum_System.Repositories.Contracts;
using Forum_System.Services.Contracts;

namespace Forum_System.Services
{
	public class TagService : ITagService
	{
		private readonly ITagRepository repository;
		private readonly IUserRepository userRepository;
		private readonly IAuthService authService;

        public TagService(ITagRepository repository, IUserRepository userRepository, IAuthService authService)
        {
			this.repository = repository;
			this.userRepository = userRepository;
			this.authService = authService;
        }

		public IList<Tag> GetAll()
		{
			return repository.GetAll();
		}
		public Tag GetById(int id)
		{
			return repository.GetById(id);
		}

		public Tag GetByName(string name)
		{
			return repository.GetByName(name);
		}

		public Tag Create(Tag tag)
		{
			return repository.Create(tag);
		}

		public bool Delete(int loggedId, int id)
		{
			var loggedUser = userRepository.GetById(loggedId);
			authService.CheckAdminAuthorization(loggedUser);

			return repository.Delete(id);
		}
	}
}
