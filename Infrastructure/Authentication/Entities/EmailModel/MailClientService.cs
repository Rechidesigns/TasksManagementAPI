namespace TasksManagementAPI.Infrastructure.Authentication.Entities.EmailModel
{
    public class MailClientService
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}

