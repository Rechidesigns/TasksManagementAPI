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
                        Message = "There was an error creating this user. Please check your input and try again. " +
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


        public async Task<ResponseDto<ApplicationUser>> GetUserByEmailAsync(string email)
        {
            var user =  await _context.ApplicationUsers.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                return new ResponseDto<ApplicationUser>
                {
                    Data = null,
                    Message = "User email Not Found"
                };
            }

            return new ResponseDto<ApplicationUser>
            {
                Data = user,
                Message = "User email found successfully"
            };
        }

        public async Task<ResponseDto<ApplicationUser>> UpdateUserAsync(UserEditDto userEditDto, string userId)
        {
            var user = await _context.ApplicationUsers.FindAsync(userId);

            if (user == null)
            {
                return new ResponseDto<ApplicationUser>
                {
                    Data = null,
                    Message = "User with this ID does not exist"
                };
            }

            user.FirstName = userEditDto.FirstName;
            user.LastName = userEditDto.LastName;
            user.PhoneNumber = userEditDto.PhoneNumber;

            await _context.SaveChangesAsync();

            return new ResponseDto<ApplicationUser>
            {
                Data = user,
                Message = "User details has been updated successfully"
            };
        }


        public async Task<ResponseDto<string>> DeleteUserAsync(string userId)
        {
            var user = await _context.ApplicationUsers.FindAsync(userId);

            if (user == null)
            {
                return new ResponseDto<string>
                {
                    Data = null,
                    Message = "User does not exist"
                };
            }

            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();

            return new ResponseDto<string>
            {
                Data = userId,
                Message = " Your account has been deleted successfully"
            };
        }

        public async Task<ResponseDto<ApplicationUser>> GetUserByUserIdAsync(string userId)
        {
            var user = await _context.ApplicationUsers.FindAsync(userId);

            if (user == null)
            {
                return new ResponseDto<ApplicationUser>
                {
                    Data = null,
                    Message = "User with this details Not Found"
                };
            }

            return new ResponseDto<ApplicationUser>
            {
                Data = user,
                Message = "User details found successfully"
            };
        }
    }
}
