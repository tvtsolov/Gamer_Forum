using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.ViewМodels
{
	public class LoginViewModel
	{
		[Required]
        public string Username { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
