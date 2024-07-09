using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;

namespace TasksManagementAPI.Core.Interface
{
    public interface ITaskService
    {
        Task<ResponseDto<TaskManager >> CreateTaskAsync(TaskDto taskDto);
        Task<ResponseDto<IEnumerable<TaskManager>>> GetAllTaskAsync();
        Task<ResponseDto<TaskManager>> GetTaskByIdAsync(string id);
        Task<ResponseDto<TaskManager>> UpdateTaskAsync( string id, TaskDto taskDto);
        Task<ResponseDto<string>> DeleteTaskAsync(string id);
    }
}
