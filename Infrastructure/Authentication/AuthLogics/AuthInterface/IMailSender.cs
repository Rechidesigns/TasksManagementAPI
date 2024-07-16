
namespace TasksManagementAPI.Infrastructure.Authentication.AuthLogics.AuthInterface
{
    public interface IMailSender
    {
        Task ForgotPassword(EmailMessageModel model);
        Task ChangePassword(EmailMessageModel model);
        Task Register(EmailMessageModel model);
        Task VerifyEmail(EmailMessageModel model);
    }
}
