using AutomotiveForumSystem.Models;

namespace AutomotiveForumSystem.Services.Contracts
{
    public interface IUsersService
    {
        User Create(User user);

        User GetById(int id);

        User GetByUsername(string username);

        User GetByEmail(string email);

        List<User> GetByFirstName(string firstName);

        User Block(User user);

        User Unblock(User user);

        User SetAdmin(User user);

        User UpdateProfileInformation(int id, User user);

        void Delete(User userToDelete);

        List<User> GetAll();
    }
}
