using System.ComponentModel.DataAnnotations;

namespace TasksManagementAPI.Infrastructure.Authentication.Entities.AuthDto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
