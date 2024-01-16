using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models.Contracts
{
	public interface ILike
	{
        int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
		public bool IsDeleted { get; set; }
	}
}
