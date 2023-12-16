﻿using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOS;

namespace AutomotiveForumSystem.Helpers
{
    public class UserMapper : IUserMapper
    {
        public User Map(UserCreateDTO user)
        {
            return new User()
            {
                UserName = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}