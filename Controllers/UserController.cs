using Azure;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register (UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.CreateUserAsync(userDto);

            if (response.Data == null)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<ResponseDto<ApplicationUser>>> GetUserByEmail (string email)
        {
            var response = await _userService.GetUserByEmailAsync(email);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);

        }

        [HttpPatch("{userId}")]
        public async Task<ActionResult<ResponseDto<ApplicationUser>>> UpdateUser (UserEditDto userEditDto, string userId)
        {
            var response = await _userService.UpdateUserAsync(userEditDto, userId);

            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<ResponseDto<string>>> DeleteUser (string userId)
        {
            var response = await _userService.DeleteUserAsync(userId);

            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ResponseDto<ApplicationUser>>> GetUserByUserId (string userId)
        {
            var response = await _userService.GetUserByUserIdAsync(userId);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);

        }
    }
}
