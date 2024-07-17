using Microsoft.EntityFrameworkCore;
using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;
using TasksManagementAPI.Core.Interface;
using TasksManagementAPI.Data;

namespace TasksManagementAPI.Core.Services
{
    public class TasksServices : ITasksService
    {

        private readonly AppDbContext _context;

        public TasksServices (AppDbContext context)
        {
            _context = context;
        }
        //public async Task<ResponseDto<Tasks>> AssignTaskAsync(string assignedToUserId, TaskDto taskDto)
        //{
        //    var tasks = new Tasks
        //    {
        //        Title = taskDto.Title,
        //        Description = taskDto.Description,
        //        DueDate = taskDto.DueDate,
        //        Status = taskDto.Status,
        //        UserId = assignedToUserId
        //    };

        //    _context.Tasks.Add(tasks);
        //    await _context.SaveChangesAsync();

        //    return new ResponseDto<Tasks>
        //    {
        //        Data = tasks,
        //        Message = "Task Assigned Successfully",
        //    };
        //}


        public async Task<ResponseDto<IEnumerable<Tasks>>> GetAllTaskAsync(string userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new ResponseDto<IEnumerable<Tasks>>
                    {
                        Data = null,
                        Message = "User does not exist."
                    };
                }

                var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();

                return new ResponseDto<IEnumerable<Tasks>>
                {
                    Data = tasks,
                    Message = "Tasks retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<Tasks>>
                {
                    Data = null,
                    Message = $"An error occurred while retrieving tasks: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDto<Tasks>> GetTaskByTaskIdAsync(string userId, string taskId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new ResponseDto<Tasks>
                    {
                        Data = null,
                        Message = "User does not exist."
                    };
                }

                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
                if (task == null)
                {
                    return new ResponseDto<Tasks>
                    {
                        Data = null,
                        Message = "Task not found."
                    };
                }

                return new ResponseDto<Tasks>
                {
                    Data = task,
                    Message = "Task retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<Tasks>
                {
                    Data = null,
                    Message = $"An error occurred while retrieving the task: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDto<Tasks>> UpdateTaskAsync(string userId, string taskId, TaskDto taskDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new ResponseDto<Tasks>
                    {
                        Data = null,
                        Message = "User does not exist."
                    };
                }

                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
                if (task == null)
                {
                    return new ResponseDto<Tasks>
                    {
                        Data = null,
                        Message = "Task not found."
                    };
                }

                task.Title = taskDto.Title;
                task.Description = taskDto.Description;
                task.DueDate = taskDto.DueDate;
                task.Status = taskDto.Status;

                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();

                return new ResponseDto<Tasks>
                {
                    Data = task,
                    Message = "Task updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<Tasks>
                {
                    Data = null,
                    Message = $"An error occurred while updating the task: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDto<string>> DeleteTaskAsync(string userId, string taskId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new ResponseDto<string>
                    {
                        Data = null,
                        Message = "User does not exist."
                    };
                }

                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
                if (task == null)
                {
                    return new ResponseDto<string>
                    {
                        Data = null,
                        Message = "Task not found."
                    };
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return new ResponseDto<string>
                {
                    Data = taskId,
                    Message = "Task deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<string>
                {
                    Data = null,
                    Message = $"An error occurred while deleting the task: {ex.Message}"
                };
            }
        }

        public async Task<ResponseDto<Tasks>> CreateTaskAsync(string userId, TaskDto taskDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new ResponseDto<Tasks>
                    {
                        Data = null,
                        Message = "User does not exist."
                    };
                }

                var task = new Tasks
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    DueDate = taskDto.DueDate,
                    Status = taskDto.Status,
                    UserId = userId
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return new ResponseDto<Tasks>
                {
                    Data = task,
                    Message = "Task created successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<Tasks>
                {
                    Data = null,
                    Message = $"An error occurred while creating the task: {ex.Message}"
                };
            }
        }

    }
}
