using AutomotiveForumSystem.Models.DTOs;

namespace AutomotiveForumSystem.Models.ViewModels
{
	public class PostDataViewModel
	{
		public int Id { get; set; }
		public string CategoryName { get; set; }

		public string Title { get; set; } = "";

		public string Content { get; set; } = "";

		public string CreateDate { get; set; }

		public string CreatedBy { get; set; }

		public int Likes { get; set; }

		public List<Comment> Comments { get; set; } = new();
	}
}
