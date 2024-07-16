using AuthService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksManagementAPI.Core.Interface;
using TasksManagementAPI.Core.Services;
using TasksManagementAPI.Infrastructure.Authentication.AuthLogics.AuthInterface;
using TasksManagementAPI.Infrastructure.Authentication.Entities.AuthDto;
using TasksManagementAPI.Infrastructure.Authentication.Entities.AuthModel;

namespace TasksManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMailSender _mailSender;

        public AuthController (IAuthService authService, IMailSender mailSender)
        {
            _authService = authService;
            _mailSender = mailSender;
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(c => c.Errors.Select(d => d.ErrorMessage)).ToList();
                var modelResponse = Result<LoginResponseDto>.ValidationError(errors);
                return BadRequest(modelResponse);
            }
            var response = await _authService.Login(model);
            if (!response.Succeeded) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenNewRequestModel model)
        {

            var response = await _authService.RefreshToken(model);
            if (!response.Succeeded) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost]
        [Route("change-password/{email}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromRoute] string email, [FromBody] ChangePassword model)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(c => c.Errors.Select(d => d.ErrorMessage)).ToList();
                return BadRequest(errors);
            }

            var result = await _authService.ChangePassword(email, model);
            if (result != null)
            {
                return Ok(new JsonMessage<string>()
                {
                    status = true,
                    success_message = "Successfully Changed Password"
                });

            }
            return Ok(new JsonMessage<string>()
            {
                error_message = result,
                status = false
            });
        }

        [HttpPost]
        [Route("logout")]
        // [AllowAnonymous]
        [Authorize]

        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto logoutRequest)
        {
            if (string.IsNullOrEmpty(logoutRequest.AccessToken))
            {
                return BadRequest(new { succeeded = false, error = "Access token is required." });
            }

            var response = await _authService.Logout(logoutRequest.AccessToken);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
