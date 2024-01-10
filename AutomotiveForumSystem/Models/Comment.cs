using AutomotiveForumSystem.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomotiveForumSystem.Models
{
    public class Comment : IComment
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(32), MaxLength(8192)]
        public string Content { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; }

        [Required]
        public int PostID { get; set; }

        [ForeignKey(nameof(PostID))]
        public Post Post { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

		public int? ParentCommentId { get; set; }

		[ForeignKey(nameof(ParentCommentId))]
		public IList<Comment> Replies { get; set; } = new List<Comment>();

        [Required]
        public bool IsDeleted { get; set; }
    }
}