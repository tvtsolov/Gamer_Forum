using Forum_System.Data;
using Forum_System.Exceptions;
using Forum_System.Models;
using Forum_System.Models.Contracts;
using Forum_System.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Forum_System.Repositories
{
	public class TagRepository : ITagRepository
	{
		private readonly ApplicationContext context;

		public TagRepository(ApplicationContext context)
		{
			this.context = context;
		}

		public IList<Tag> GetAll()
		{
			return context.Tags.ToList();
		}

		private IQueryable<Tag> GetTags()
		{
			return context.Tags.Include(t => t.Threads);
		}

		//Add SortBy?

		public Tag Create(Tag tag)
		{
			//Maybe shouldn't throw exception?
			CheckDuplicateTagName(tag.Name);

			context.Tags.Add(tag);
			context.SaveChanges();

			return tag;
		}

		public bool Delete(int id)
		{
			var tag = GetById(id);
			context.Tags.Remove(tag);

			return context.SaveChanges() > 0;
		}


		public Tag GetById(int id)
		{
			var tag = GetTags().FirstOrDefault(t => t.Id == id);

			return tag ?? throw new EntityNotFoundException($"Tag with ID - {id} not found!");
		}


		public Tag GetByName(string name)
		{
			var tag = context.Tags.FirstOrDefault(t => t.Name == name);

			return tag ?? throw new EntityNotFoundException($"Tag with name - {name} not found!");
		}

		public void CheckDuplicateTagName(string name)
		{
			var tag = context.Tags.FirstOrDefault(t => t.Name == name);

			if (tag != null)
			{
				throw new DuplicateEntityException($"Tag {name} already exists!");
			}
		}
	}
}
