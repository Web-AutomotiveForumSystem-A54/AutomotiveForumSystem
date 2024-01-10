using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Services.Contracts;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Models.DTOs;

namespace AutomotiveForumSystem.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository users;

        public UsersService(IUsersRepository users)
        {
            this.users = users;
        }

        public User Create(User user)
        {
            this.EnsureUsernameIsUnique(user.UserName);
            this.EnsureEmailIsUnique(user.Email);
            return this.users.Create(user);
        }

        public User GetById(int id)
        {
            return this.users.GetById(id);
        }

        public User GetByUsername(string username)
        {
            return this.users.GetByUsername(username);
        }

        public User GetByEmail(string email)
        {
            return this.users.GetByEmail(email);
        }

        public List<User> GetByFirstName(string firstName)
        {
            return this.users.GetByFirstName(firstName);
        }

        public User Block(User user)
        {
            this.CheckUserDeleted(user);
            this.CheckUserBlocked(user);

            return this.users.Block(user);
        }

        public User Unblock(User user)
        {
            this.CheckUserDeleted(user);
            this.CheckUserUnblocked(user);

            return this.users.Unblock(user);
        }

        public User SetAdmin(User user)
        {
            this.CheckUserAdmin(user);
            this.CheckUserDeleted(user);
            this.CheckUserBlocked(user);

            return this.users.SetAdmin(user);
        }

        public User UpdateProfileInformation(User user, UserUpdateProfileInformationDTO userDTO)
        {
            if (user.IsBlocked) throw new UserBlockedException("Your account is blocked.");
            if (user.IsDeleted) throw new EntityNotFoundException("Account does not exists.");

            this.EnsureEmailIsUnique(userDTO.Email);
            return this.users.UpdateProfileInformation(user, userDTO);
        }

        public void Delete(User userToDelete)
        {
            this.CheckUserDeleted(userToDelete);
            this.users.Delete(userToDelete);
        }
        
        private void EnsureUsernameIsUnique(string username)
        {
            var user = this.users.GetAll().FirstOrDefault(u => u.UserName == username);

            if (user != null)
                throw new DuplicateEntityException($"Username {username} is already used.");
        }

        private void EnsureEmailIsUnique(string email)
        {
            var user = this.users.GetAll().FirstOrDefault(u => u.Email == email);

            if (user != null)
                throw new DuplicateEntityException($"Email {email} is already used.");
        }       

        private void CheckUserUnblocked(User user)
        {
            if (!user.IsBlocked)
                throw new UserNotBlockedException("User is not blocked.")
;        }

        private void CheckUserBlocked(User user)
        {
            if (user.IsBlocked) 
                throw new UserBlockedException("User is already blocked.");
        }

        private void CheckUserDeleted(User user)
        {
            if (user.IsDeleted) 
                throw new EntityNotFoundException("Account does not exist.");
        }

        private void CheckUserAdmin(User user)
        {
            if (user.IsAdmin)
                throw new AdminRightsAlreadyGrantedException("User has already been granted with admin rights.");
        }

        public List<User> GetAll()
        {
            return this.users.GetAll();
        }
    }
}