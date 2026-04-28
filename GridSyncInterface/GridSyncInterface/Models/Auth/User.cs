using System.ComponentModel.DataAnnotations;

namespace GridSyncInterface.Models.Auth
{
    /// <summary>
    /// Application user stored in the database.
    /// Passwords are stored as BCrypt hashes – never plain text.
    /// </summary>
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        /// <summary>BCrypt hash of the user's password.</summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = UserRoles.Viewer;   // Admin | Editor | Viewer

        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? FullName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // Refresh token support
        [MaxLength(512)]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        // Navigation
        public ICollection<ProjectMembership> ProjectMemberships { get; set; } = new List<ProjectMembership>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Editor = "Editor";
        public const string Viewer = "Viewer";
    }
}
