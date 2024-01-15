using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomotiveForumSystem.Models.ViewModels
{
	public class PostCreateViewModel
	{
		public string Title { get; set; } = "";
		public string Content { get; set; } = "";
		public int UserID { get; set; }
		public int CategoryID { get; set; }
		public SelectList Categories { get; set; }
	}
}
