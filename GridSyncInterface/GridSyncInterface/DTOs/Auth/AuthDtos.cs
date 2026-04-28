using System.ComponentModel.DataAnnotations;

namespace GridSyncInterface.DTOs.Auth
{
    public class LoginRequest
    {
        [Required] public string Username { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required, MaxLength(100)] public string Username { get; set; } = string.Empty;
        [Required, MinLength(8)]   public string Password { get; set; } = string.Empty;
        [Required, MaxLength(50)]  public string Role { get; set; } = "Viewer";
        [MaxLength(200)]           public string? Email { get; set; }
        [MaxLength(200)]           public string? FullName { get; set; }
    }

    public class ChangePasswordRequest
    {
        [Required] public string CurrentPassword { get; set; } = string.Empty;
        [Required, MinLength(8)] public string NewPassword { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest
    {
        [Required] public string RefreshToken { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string AccessToken  { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt  { get; set; }
        public UserDto User        { get; set; } = new();
    }

    public class UserDto
    {
        public int    Id        { get; set; }
        public string Username  { get; set; } = string.Empty;
        public string Role      { get; set; } = string.Empty;
        public string? Email    { get; set; }
        public string? FullName { get; set; }
        public bool   IsActive  { get; set; }
        public DateTime CreatedAt   { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class UpdateUserRequest
    {
        [Required, MaxLength(50)]  public string Role     { get; set; } = "Viewer";
        [MaxLength(200)]           public string? Email   { get; set; }
        [MaxLength(200)]           public string? FullName { get; set; }
    }

    public class AdminResetPasswordRequest
    {
        [Required, MinLength(8)] public string NewPassword { get; set; } = string.Empty;
    }
}
