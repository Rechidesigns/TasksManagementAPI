
using TasksManagementAPI.Infrastructure.Shared;

namespace TasksManagementAPI.Infrastructure.Authentication.AuthLogics.AuthInterface

{
    public class SentEmailOtp : BaseEntity
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
    }
}