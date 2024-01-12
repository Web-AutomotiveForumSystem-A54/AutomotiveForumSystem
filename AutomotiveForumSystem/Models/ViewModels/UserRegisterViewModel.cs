using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models.ViewModels
{
	public class UserRegisterViewModel
	{
        [Required, MinLength(4), MaxLength(16)]
        public string Username { get; set; }

        [Required, MinLength(4, ErrorMessage = "Password should be more than 4 symbols."), MaxLength(32, ErrorMessage = "Password should be no more than 32 symbols.")]
        public string Password { get; set; }

		[Required, MinLength(4, ErrorMessage = "First name should be more than 4 symbols."), MaxLength(32, ErrorMessage = "First name should be no more than 32 symbols.")]
		public string FirstName { get; set; }

		[Required, MinLength(4, ErrorMessage = "Last name should be more than 4 symbols."), MaxLength(32, ErrorMessage = "Last name should be no more than 32 symbols.")]
		public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
