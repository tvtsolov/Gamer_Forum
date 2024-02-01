using Forum_System.Models.Contracts;

namespace Forum_System.Models
{
	public class Tag : ITag
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Thread> Threads { get; set; }
	}
}
