﻿using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Repositories.Contracts;

namespace AutomotiveForumSystem.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationContext context;

        public UsersRepository(ApplicationContext context)
        {
            this.context = context;
        }        

        public User Create(User user)
        {
            this.context.Users.Add(user);
            this.context.SaveChanges();

            return user;
        }

        public List<User> GetAll()
        {
            return this.context.Users.ToList();
        }

        public User GetById(int id)
        {
            return this.GetAll().FirstOrDefault(u => u.Id == id)
                ?? throw new EntityNotFoundException($"User with id {id} does not exist.");
        }

        public User GetByUsername(string username)
        {
            return this.GetAll().FirstOrDefault(u => u.Username == username)
                ?? throw new EntityNotFoundException($"User with username {username} does not exist.");
        }

        public User GetByEmail(string email)
        {
            return this.GetAll().FirstOrDefault(u => u.Email == email)
                ?? throw new EntityNotFoundException($"User with email {email} does not exist.");
        }

        public List<User> GetByFirstName(string firstName)
        {
            var users = this.GetAll().FindAll(u => u.FirstName == firstName);

            if (users.Count == 0)
            {
                throw new EntityNotFoundException($"No users with first name {firstName} exist.");
            }

            return users;
        }

        public User Block(User user)
        {
            user.IsBlocked = true;
            this.context.SaveChanges();
            return user;
        }

        public User Unblock(User user)
        {
            user.IsBlocked = false;
            this.context.SaveChanges();
            return user;
        }

        public User SetAdmin(User user)
        {
            user.IsAdmin = true;
            this.context.SaveChanges();
            return user;
        }

        public User UpdateProfileInformation(int id, User user)
        {
            var userToUpdate = this.GetById(id);

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            userToUpdate.PhoneNumber = user.PhoneNumber;

            this.context.SaveChanges();

            return userToUpdate;
        }


        public void Delete(User userToDelete)
        {
            userToDelete.IsDeleted = true;
            this.context.SaveChanges();
        }        
    }
}
