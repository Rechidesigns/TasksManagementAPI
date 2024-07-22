using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Services;

namespace TasksManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TasksServices _tasksServices;

        public TaskController(TasksServices tasksServices)
        {
            _tasksServices = tasksServices;
        }

        [HttpGet("{userId}/all-tasks")]
        [Authorize]
        public async Task<IActionResult> GetAllTasks(string userId)
        {
            var response = await _tasksServices.GetAllTaskAsync(userId);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{userId}/tasks/{taskId}")]
        [Authorize]
        public async Task<IActionResult> GetTaskById(string userId, string taskId)
        {
            var response = await _tasksServices.GetTaskByTaskIdAsync(userId, taskId);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("{userId}/tasks/{taskId}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(string userId, string taskId, [FromBody] TaskDto taskDto)
        {
            var response = await _tasksServices.UpdateTaskAsync(userId, taskId, taskDto);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{userId}/tasks/{taskId}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(string userId, string taskId)
        {
            var response = await _tasksServices.DeleteTaskAsync(userId, taskId);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("{userId}/tasks")]
        [Authorize]
        public async Task<IActionResult> CreateTask(string userId, [FromBody] TaskDto taskDto)
        {
            var response = await _tasksServices.CreateTaskAsync(userId, taskDto);
            if (response.Data == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}

