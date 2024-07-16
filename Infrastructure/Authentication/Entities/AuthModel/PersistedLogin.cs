using System.ComponentModel.DataAnnotations;

namespace TasksManagementAPI.Infrastructure.Authentication.Entities.AuthModel
{
    public class PersistedLogin
    {
        [Key]
        [Required] public Guid UserId { get; set; }
        [Required] public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedOn { get; set; }
        [Required, StringLength(256)] public string RefreshToken { get; set; }
        [Required] public DateTime RefreshTokenExpiryTime { get; set; }
    }
}