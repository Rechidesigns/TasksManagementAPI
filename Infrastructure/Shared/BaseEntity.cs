﻿namespace TasksManagementAPI.Infrastructure.Shared
{
    public class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.ToLocalTime();
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow.ToLocalTime();
    }
}
