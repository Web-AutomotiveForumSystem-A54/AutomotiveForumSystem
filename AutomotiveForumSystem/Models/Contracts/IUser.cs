namespace AutomotiveForumSystem.Models.Contracts
{
    public interface IUser
    {
        int Id { get; set; }
        string Username { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string? PhoneNumber { get; set; }
        IList<Post> Posts { get; set; }
        IList<Comment> Comments { get; set; }
        bool IsAdmin { get; set; }
        bool IsBlocked { get; set; }
        bool IsDeleted { get; set; }
    }
}
