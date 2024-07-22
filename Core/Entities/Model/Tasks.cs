﻿using System.Text.Json.Serialization;
using TasksManagementAPI.Infrastructure.Shared;

namespace TasksManagementAPI.Core.Entities.Model
{
    public class Tasks : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }

        [JsonIgnore] // Ignore the User property to avoid cyclic reference
        public ApplicationUser User { get; set; }  // Navigation property to the user
    }
}