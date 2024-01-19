using AutomotiveForumSystem.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomotiveForumSystem.Models
{
    public class Post : IPost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category Category { get; set; }

        [Required, MinLength(16), MaxLength(64)]
        public string Title { get; set; }

        [Required, MinLength(32), MaxLength(8192)]
        public string Content { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; }

        [Required]
        public IList<Tag> Tags { get; set; } = new List<Tag>();
        
        [Required]
        public DateTime CreateDate { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        [Required]
        public IList<Like> Likes { get; set; } = new List<Like>();

        public int TotalLikesCount { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
	}
}
