using System.ComponentModel.DataAnnotations;
using Forum_System.Helpers;

namespace Forum_System.Models.ViewМodels
{
    public class AvatarViewModel
    {
        public string FileName { set; get; }
        public IFormFile Picture { set; get; }
    }
}
