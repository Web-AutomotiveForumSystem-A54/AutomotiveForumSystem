using AutomotiveForumSystem.Models;

namespace AutomotiveForumSystem.Repositories.Contracts
{
    public interface IUsersRepository
    {
        User Create(User user);

        User GetById(int id);

        List<User> GetAll();

        User GetByUsername(string username);

        User GetByEmail(string email);

        List<User> GetByFirstName(string firstName);

        User Block(User user);

        User Unblock(User user);

        User SetAdmin(User user);

        User UpdateProfileInformation(int id, User user);

        void Delete(User userToDelete);
    }
}
