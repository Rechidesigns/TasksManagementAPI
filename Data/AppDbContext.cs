using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;

namespace TasksManagementAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        internal object taskManager;

        // the IdentityDbContext is inheriting from the Identity user from
        // entity framework core, pre built in.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // register your models in the database context.
        public DbSet <ApplicationUser> applicationUsers { get; set; }
        public DbSet <TaskManager> taskManagers { get; set; }

    }

}
