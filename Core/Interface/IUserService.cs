using TasksManagementAPI.Core.Entities.Dto;
using TasksManagementAPI.Core.Entities.Model;

namespace TasksManagementAPI.Core.Interface
{
    public interface IUserService
    {
        Task<ResponseDto<ApplicationUser>> GetUserByEmailAsync (string email);
        Task<ResponseDto<ApplicationUser>> GetUserByUserIdAsync (string userId);
        Task<ResponseDto<ApplicationUser>> CreateUserAsync (UserDto userDto);
        Task<ResponseDto<ApplicationUser>> UpdateUserAsync (UserEditDto userEditDto, string userId);
        Task<ResponseDto<string>> DeleteUserAsync (string userId);
    }
}
