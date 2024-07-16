using TasksManagementAPI.Infrastructure.Authentication.Entities.EmailModel;

namespace TasksManagementAPI.Infrastructure.Authentication.AuthLogics.AuthInterface
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequestService mailRequest);

    }
}
