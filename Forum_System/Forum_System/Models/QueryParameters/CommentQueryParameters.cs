namespace Forum_System.Models.QueryParameters
{
    public class CommentQueryParameters
    {
        public string? Author { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ThreadTitle { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public string? Content { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        //public string? CreationDate { get; set; }
        //public string? CreatedAfter { get; set; }
        //public string? CreatedBefore { get; set; }

        // filter by thread id?
    }
}
