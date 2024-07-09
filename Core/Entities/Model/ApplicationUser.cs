﻿using Microsoft.AspNetCore.Identity;

namespace TasksManagementAPI.Core.Entities.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }

    }
}