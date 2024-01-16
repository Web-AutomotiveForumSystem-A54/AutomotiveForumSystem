using AutomotiveForumSystem.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models.ViewModels
{
	public class PostDataViewModel
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }

		[Required, MinLength(16), MaxLength(64)]
		public string Title { get; set; } = "";

		[Required, MinLength(32), MaxLength(8192)]
		public string Content { get; set; } = "";

		public string CreateDate { get; set; }

		public string CreatedBy { get; set; }

		public List<Comment> Comments { get; set; } = new();
		public List<Like> Likes { get; set; } = new();

		public CommentCreateDTO Comment { get; set; } = new CommentCreateDTO();
	}
}
