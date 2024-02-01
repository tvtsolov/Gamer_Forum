using Swashbuckle.AspNetCore.Annotations;

namespace Forum_System.Models.QueryParameters
{
    public class ThreadQueryParameters
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }

        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
        
        public int? MaxRating { get; set; }
        public int? MinRating { get; set;}

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
