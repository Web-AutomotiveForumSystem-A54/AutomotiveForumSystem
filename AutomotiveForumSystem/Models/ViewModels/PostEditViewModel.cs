using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models.ViewModels
{
	public class PostEditViewModel
	{
		public int Id { get; set; }
		[Required, MinLength(16), MaxLength(64)]
		public string Title { get; set; } = "";

		[Required, MinLength(32), MaxLength(8192)]
		public string Content { get; set; } = "";
		public int UserID { get; set; }
		public int CategoryID { get; set; }
		public SelectList Categories { get; set; }
		public string Tags { get; set; }
	}
}
