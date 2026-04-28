using System.Security.Claims;
using GridSyncInterface.Services;
using Microsoft.AspNetCore.Mvc;

namespace GridSyncInterface.Controllers
{
    /// <summary>
    /// Shared base – parses the JWT caller identity and translates
    /// service exceptions to the correct HTTP status codes.
    /// </summary>
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        protected int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new InvalidOperationException("User identity not found."));

        protected string CurrentUsername =>
            User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        /// <summary>
        /// Wraps a service call and maps domain exceptions ? HTTP responses.
        /// </summary>
        protected async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ElementLockedException ex)
            {
                return Conflict(new { error = ex.Message, type = "ElementLocked" });
            }
            catch (ConcurrencyConflictException ex)
            {
                return Conflict(new { error = ex.Message, type = "ConcurrencyConflict" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
