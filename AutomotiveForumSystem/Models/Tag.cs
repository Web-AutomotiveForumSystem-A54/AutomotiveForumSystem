using AutomotiveForumSystem.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models
{
	public class Tag : ITag
	{
		[Key]
		public int Id { get; set; }

		[Required, MinLength(3), MaxLength(15)]
		public string Name { get; set; }

		[Required]
		public bool IsDeleted { get; set; }

		[Required]
		public IList<Post> Posts { get; set; } = new List<Post>();
	}
}
