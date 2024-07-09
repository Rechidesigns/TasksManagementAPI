using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;
using TasksManagementAPI.Core.Interface;

namespace TasksManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagerController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskManagerController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<TaskManager>>>> GetAllTask()
        {
            var response = await _taskService.GetAllTaskAsync();

            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<TaskManager>>> GetTaskById(string id)
        {
            var response = await _taskService.GetTaskByIdAsync(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<TaskManager>>> CreateTask(TaskDto taskDto)
        {
            var response = await _taskService.CreateTaskAsync(taskDto);
            return CreatedAtAction(nameof(GetTaskById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<TaskManager>>> UpdateTask(string id, TaskDto taskDto)
        {
            var response = await _taskService.UpdateTaskAsync(id, taskDto);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<string>>> DeleteTask (string id)
        {
            var response = await _taskService.DeleteTaskAsync(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}

