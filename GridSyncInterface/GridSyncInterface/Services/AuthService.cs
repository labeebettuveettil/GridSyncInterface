using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GridSyncInterface.Data;
using GridSyncInterface.DTOs.Auth;
using GridSyncInterface.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GridSyncInterface.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshAsync(RefreshTokenRequest request);
        Task<UserDto> RegisterAsync(RegisterRequest request);
        Task ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task SetActiveAsync(int userId, bool active);
        Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request);
        Task AdminResetPasswordAsync(int userId, AdminResetPasswordRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly SclDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(SclDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // ?? Login ????????????????????????????????????????????????????????????
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive)
                ?? throw new UnauthorizedAccessException("Invalid username or password.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            return await BuildAuthResponseAsync(user);
        }

        // ?? Refresh token ????????????????????????????????????????????????????
        public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u =>
                    u.RefreshToken == request.RefreshToken &&
                    u.RefreshTokenExpiry > DateTime.UtcNow &&
                    u.IsActive)
                ?? throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            return await BuildAuthResponseAsync(user);
        }

        // ?? Register ?????????????????????????????????????????????????????????
        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            if (await _db.Users.AnyAsync(u => u.Username == request.Username))
                throw new InvalidOperationException($"Username '{request.Username}' is already taken.");

            var user = new AppUser
            {
                Username     = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role         = request.Role,
                Email        = request.Email,
                FullName     = request.FullName,
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return MapUserDto(user);
        }

        // ?? Change password ??????????????????????????????????????????????????
        public async Task ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            var user = await _db.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            user.PasswordHash   = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.RefreshToken   = null;   // invalidate existing sessions
            user.RefreshTokenExpiry = null;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _db.Users.ToListAsync();
            return users.Select(MapUserDto);
        }

        public async Task SetActiveAsync(int userId, bool active)
        {
            var user = await _db.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException("User not found.");
            user.IsActive = active;
            await _db.SaveChangesAsync();
        }

        // ?? Helpers ??????????????????????????????????????????????????????????
        private async Task<AuthResponse> BuildAuthResponseAsync(AppUser user)
        {
            var expiry       = DateTime.UtcNow.AddMinutes(GetJwtExpiryMinutes());
            var accessToken  = GenerateJwt(user, expiry);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            user.LastLoginAt        = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken  = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt    = expiry,
                User         = MapUserDto(user)
            };
        }

        private string GenerateJwt(AppUser user, DateTime expiry)
        {
            var key    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetJwtSecret()));
            var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.Username),
                new Claim(ClaimTypes.Role,           user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer:   _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims:   claims,
                expires:  expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string GetJwtSecret()
            => _config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret not configured.");

        private int GetJwtExpiryMinutes()
            => int.TryParse(_config["Jwt:ExpiryMinutes"], out var m) ? m : 60;

        public static UserDto MapUserDto(AppUser u) => new()
        {
            Id          = u.Id,
            Username    = u.Username,
            Role        = u.Role,
            Email       = u.Email,
            FullName    = u.FullName,
            IsActive    = u.IsActive,
            CreatedAt   = u.CreatedAt,
            LastLoginAt = u.LastLoginAt
        };

        // ?? Update user profile (role / email / fullName) ??????????????????
        public async Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request)
        {
            var user = await _db.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException($"User {userId} not found.");

            user.Role     = request.Role;
            user.Email    = request.Email;
            user.FullName = request.FullName;

            await _db.SaveChangesAsync();
            return MapUserDto(user);
        }

        // ?? Admin password reset (no current-password check) ?????????????????
        public async Task AdminResetPasswordAsync(int userId, AdminResetPasswordRequest request)
        {
            var user = await _db.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException($"User {userId} not found.");

            user.PasswordHash      = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.RefreshToken      = null;   // invalidate any existing sessions
            user.RefreshTokenExpiry = null;

            await _db.SaveChangesAsync();
        }
    }
}
