using TasksManagementAPI.Infrastructure.Shared;

namespace TasksManagementAPI.Core.Entities.Model
{
    public class TaskManager : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }
}
