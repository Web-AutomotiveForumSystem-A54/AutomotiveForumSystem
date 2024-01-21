﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomotiveForumSystem.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public IList<PostPreViewModel> Posts { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsBlocked { get; set; }
    }
}
