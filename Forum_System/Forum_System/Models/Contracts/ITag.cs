namespace Forum_System.Models.Contracts
{
	public class ITag
	{
		int Id { get; set; }
		string Name { get; set; }
		List<Thread> Threads { get; set; }
	}
}
