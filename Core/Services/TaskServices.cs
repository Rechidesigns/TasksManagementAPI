using Microsoft.EntityFrameworkCore;
using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;
using TasksManagementAPI.Core.Interface;
using TasksManagementAPI.Data;

namespace TasksManagementAPI.Core.Services
{
    public class TaskServices : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskServices (AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<TaskManager>> CreateTaskAsync(TaskDto taskDto)
        {
            var taskManager = new TaskManager
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Status = taskDto.Status
            };

            _context.TaskManagers.Add(taskManager);
            await _context.SaveChangesAsync();

            return new ResponseDto<TaskManager>
            {
                Data = taskManager,
                Message = "Task Created Successfully"
            };
        }

        public async Task<ResponseDto<string>> DeleteTaskAsync(string taskId)
        {
            var taskManager = await _context.TaskManagers.FindAsync(taskId);
            if (taskManager == null)
            {
                return new ResponseDto<string>
                {
                    Data = null,
                    Message = "Task Not Found"
                };
            }

            _context.TaskManagers.Remove(taskManager);
            await _context.SaveChangesAsync();

            return new ResponseDto<string>
            {
                Data = taskId,
                Message = "Task Deleted Successfully"
            };
        }

        public async Task<ResponseDto<IEnumerable<TaskManager>>> GetAllTaskAsync()
        {
            var taskManager = await _context.TaskManagers.ToListAsync();

            return new ResponseDto<IEnumerable<TaskManager>>
            {
                Data = taskManager,
                Message = "Tasks Successfully retrieved"
            };
        }

        public async Task<ResponseDto<TaskManager>> GetTaskByTaskIdAsync(string taskId)
        {
            var taskManager = await _context.TaskManagers.FindAsync(taskId);

            if (taskManager == null)
            {
                return new ResponseDto<TaskManager>
                {
                    Data = null,
                    Message = "Task Not Found"
                };
            }

            return new ResponseDto<TaskManager>
            {
                Data = taskManager,
                Message = "Task Returned Successfully"
            };
        }

        public async Task<ResponseDto<TaskManager>> UpdateTaskAsync(string taskId, TaskDto taskDto)
        {
            var taskManager = await _context.TaskManagers.FindAsync(taskId);

            if (taskManager == null)
            {
                return new ResponseDto<TaskManager>
                {
                    Data = null,
                    Message = "Tasks With The taskId Does Not Exist"
                };
            }

            taskManager.Title = taskDto.Title;
            taskManager.Description = taskDto.Description;
            taskManager.DueDate = taskDto.DueDate;
            taskManager.Status = taskDto.Status;

            await _context.SaveChangesAsync();

            return new ResponseDto<TaskManager>
            {
                Data = taskManager,
                Message = "Task Updated Successfully"
            };

        }
    }
}
