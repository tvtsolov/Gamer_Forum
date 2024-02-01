namespace Forum_System.Models.QueryParameters
{
    public class QueryParametersRating
    {
        public double? Rating { get; set; }
        public bool IsAddingRating { get; set; } //add or remove rating
        public int? ThreadId { get; set; }
    }
}
