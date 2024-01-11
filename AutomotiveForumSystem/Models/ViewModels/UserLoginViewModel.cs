using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models.ViewModels
{
	public class UserLoginViewModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
