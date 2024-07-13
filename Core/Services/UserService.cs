using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;
using TasksManagementAPI.Core.Interface;
using TasksManagementAPI.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TasksManagementAPI.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<ResponseDto<ApplicationUser>> CreateUserAsync(UserDto userDto)
        {
            try
            {
                var emailExists = await _context.ApplicationUsers
                    .Where(x => x.Email == userDto.Email)
                    .FirstOrDefaultAsync();

                if (emailExists != null) // Check if email already exists
                {
                    return new ResponseDto<ApplicationUser>
                    {
                        Data = null,
                        Message = $"User with the Email {userDto.Email} already exists."
                    };
                }

                var user = new ApplicationUser()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    PhoneNumber = userDto.PhoneNumber,
                    UserName = userDto.Email
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    return new ResponseDto<ApplicationUser>
                    {
                        Data = user,
                        Message = "User Account Created Successfully"
                    };
                }
                else
                {
                    return new ResponseDto<ApplicationUser>
                    {
                        Data = null,
                        Message = "There was an error creating this user. Please check your imput and try again. " +
                        "'Note' Password must contain at least 8 characters, including uppercase, lowercase, numeric digit, and special character"
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exception and return a meaningful message
                return new ResponseDto<ApplicationUser>
                {
                    Data = null,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public Task<ResponseDto<ApplicationUser>> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
