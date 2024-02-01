using Forum_System.Models;

namespace Forum_System.Services.Contracts
{
	public interface ITagService
	{
		IList<Tag> GetAll();
		Tag GetById(int id);
		Tag GetByName(string name);
		Tag Create(Tag tag);
		bool Delete(int loggedId, int id);
	}
}
