namespace Forum_System.Models.ViewМodels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Thread> ThreadsByDate { get; set; }
        public IEnumerable<Thread> ThreadsByComments { get; set; }

        public int TotalNumberOfUsers { get; set; }
        public int TotalNumberOfThreads { get; set;}

        public Comment LastComment { get; set; }
    }
}
