using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models.Contracts
{
    public interface IPost
    {
        int Id { get; set; }
        int CategoryID { get; set; }
        Category Category { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        int UserID { get; set; }
        User User { get; set; }
		IList<Tag> Tags { get; set; }
		DateTime CreateDate { get; set; }
        List<Comment> Comments { get; set; }
        IList<Like> Likes { get; set; }
        bool IsDeleted { get; set; }
    }
}
