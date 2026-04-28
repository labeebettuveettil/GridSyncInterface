using GridSyncInterface.DTOs.Auth;
using GridSyncInterface.Models.Auth;
using GridSyncInterface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GridSyncInterface.Controllers
{
    /// <summary>
    /// Authentication endpoints – login, register, refresh token, change password.
    /// </summary>
    [Route("api/auth")]
    public class AuthController : ApiBaseController
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        // POST api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
            => await ExecuteAsync(async () => Ok(await _auth.LoginAsync(request)));

        // POST api/auth/refresh
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
            => await ExecuteAsync(async () => Ok(await _auth.RefreshAsync(request)));

        // POST api/auth/register  (Admin only)
        [HttpPost("register")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
            => await ExecuteAsync(async () =>
            {
                var user = await _auth.RegisterAsync(request);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            });

        // GET api/auth/users  (Admin only)
        [HttpGet("users")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetUsers()
            => await ExecuteAsync(async () => Ok(await _auth.GetAllUsersAsync()));

        // GET api/auth/users/{id}
        [HttpGet("users/{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetUser(int id)
            => await ExecuteAsync(async () =>
            {
                var users = await _auth.GetAllUsersAsync();
                var user  = users.FirstOrDefault(u => u.Id == id);
                return user != null ? Ok(user) : NotFound();
            });

        // GET api/auth/me
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
            => await ExecuteAsync(async () =>
            {
                var users = await _auth.GetAllUsersAsync();
                var user  = users.FirstOrDefault(u => u.Id == CurrentUserId);
                return user != null ? Ok(user) : NotFound();
            });

        // PUT api/auth/me/password
        [HttpPut("me/password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
            => await ExecuteAsync(async () =>
            {
                await _auth.ChangePasswordAsync(CurrentUserId, request);
                return NoContent();
            });

        // PUT api/auth/users/{id}/active  (Admin only)
        [HttpPut("users/{id:int}/active")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> SetActive(int id, [FromQuery] bool active)
            => await ExecuteAsync(async () =>
            {
                await _auth.SetActiveAsync(id, active);
                return NoContent();
            });

        // PUT api/auth/users/{id}  (Admin only) — update role / email / fullName
        [HttpPut("users/{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
            => await ExecuteAsync(async () => Ok(await _auth.UpdateUserAsync(id, request)));

        // PUT api/auth/users/{id}/reset-password  (Admin only)
        [HttpPut("users/{id:int}/reset-password")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminResetPassword(int id, [FromBody] AdminResetPasswordRequest request)
            => await ExecuteAsync(async () =>
            {
                await _auth.AdminResetPasswordAsync(id, request);
                return NoContent();
            });
    }
}
