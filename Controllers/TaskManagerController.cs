using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;
using TasksManagementAPI.Core.Interface;
using TasksManagementAPI.Core.Services;

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

        [HttpGet("{taskId}")]
        public async Task<ActionResult<ResponseDto<TaskManager>>> GetTaskByTaskId (string taskId)
        {
            var response = await _taskService.GetTaskByTaskIdAsync(taskId);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ResponseDto<TaskManager>>> CreateTask(TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _taskService.CreateTaskAsync(taskDto);

            if (response.Data == null)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult<ResponseDto<TaskManager>>> UpdateTask(string taskId, TaskDto taskDto)
        {
            var response = await _taskService.UpdateTaskAsync(taskId, taskDto);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult<ResponseDto<string>>> DeleteTask (string taskId)
        {
            var response = await _taskService.DeleteTaskAsync(taskId);
            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}

//sagehdds