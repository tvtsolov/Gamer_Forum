using Forum_System.Models;

namespace Forum_System.Repositories.Contracts
{
	public interface ITagRepository
	{
		IList<Tag> GetAll();
		Tag GetById(int id);
		Tag GetByName(string name);
		Tag Create(Tag tag);
		bool Delete(int id);
		void CheckDuplicateTagName(string name);
	}
}
