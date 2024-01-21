using System.ComponentModel.DataAnnotations;

namespace AutomotiveForumSystem.Models.DTOs
{
    public class CommentCreateDTO
    {
        public int PostID { get; set; }
        public int? CommentID { get; set; }

        [Required, MinLength(1), MaxLength(8192)]
        public string Content { get; set; }
    }
}
