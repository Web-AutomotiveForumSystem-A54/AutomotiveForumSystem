using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models.Contracts
{
	public interface ITag
	{
		int Id { get; set; }

		string Name { get; set; }

		IList<Post> Posts { get; set; }
	}
}
