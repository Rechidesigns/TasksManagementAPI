using AuthService.Helpers;
using TasksManagementAPI.Infrastructure.Authentication.Entities.AuthDto;
using TasksManagementAPI.Infrastructure.Authentication.Entities.AuthModel;

namespace TasksManagementAPI.Infrastructure.Authentication.AuthLogics.AuthInterface
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDto>> Login(LoginDto model);
        Task<Result<LoginResponseDto>> RefreshToken(RefreshTokenNewRequestModel tokenModel);
        Task<string> ChangePassword(string email, ChangePassword model);
        Task<Result<string>> Logout(string AccessToken);

    }
}
