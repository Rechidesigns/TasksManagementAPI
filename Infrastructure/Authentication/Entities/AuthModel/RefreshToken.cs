using TasksManagementAPI.Core.Entities.Model;
using TasksManagementAPI.Infrastructure.Shared;

namespace TasksManagementAPI.Infrastructure.Authentication.Entities.AuthModel

{
    public class RefreshToken : BaseEntity
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsActive { get; set; }

        public ApplicationUser User { get; set; }
    }
}
