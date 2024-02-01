using Forum_System.Controllers.API;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Forum_System.Models.DTOs
{
    public class ThreadFilteredDto
    {
        //public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date {  get; set; }
        public double AvgRating { get; set; }
        public int CommentsCount { get; set; }
    }
}
