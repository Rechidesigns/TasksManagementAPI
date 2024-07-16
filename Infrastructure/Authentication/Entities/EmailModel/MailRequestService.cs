namespace TasksManagementAPI.Infrastructure.Authentication.Entities.EmailModel
{
    public class MailRequestService
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
