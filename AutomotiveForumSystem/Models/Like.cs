using AutomotiveForumSystem.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomotiveForumSystem.Models
{
	public class Like : ILike
	{
		[Key]
		public int Id { get; set; }

		public int UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		public int PostId { get; set; }
		[ForeignKey(nameof(PostId))]
		public Post Post { get; set; }

		[Required]
		public bool IsDeleted { get; set; }
	}
}
