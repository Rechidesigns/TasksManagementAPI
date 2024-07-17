using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;

namespace TasksManagementAPI.Core.Interface
{
    public interface ITasksService
    {
        Task<ResponseDto<Tasks>> CreateTaskAsync(string userId, TaskDto taskDto);
       // Task<ResponseDto<Tasks>> AssignTaskAsync(string assignedToUserId, TaskDto taskDto);
        Task<ResponseDto<IEnumerable<Tasks>>> GetAllTaskAsync(string userId);
        Task<ResponseDto<Tasks>> GetTaskByTaskIdAsync(string userId, string taskId);
        Task<ResponseDto<Tasks>> UpdateTaskAsync(string userId, string taskId, TaskDto taskDto);
        Task<ResponseDto<string>> DeleteTaskAsync(string userId, string taskId);
    }
}
