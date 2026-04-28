using GridSyncInterface.DTOs.Projects;
using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GridSyncInterface.Controllers
{
    /// <summary>
    /// Project lifecycle: create, read, update, delete + membership management.
    /// Also exposes audit log and element-lock endpoints scoped to a project.
    /// </summary>
    [Route("api/projects")]
    [Authorize]
    public class ProjectsController : ApiBaseController
    {
        private readonly IProjectService _projects;
        private readonly IAuditService   _audit;
        private readonly ILockService    _locks;

        public ProjectsController(IProjectService projects, IAuditService audit, ILockService locks)
        {
            _projects = projects;
            _audit    = audit;
            _locks    = locks;
        }

        // ?? CRUD ?????????????????????????????????????????????????????????????

        // GET api/projects
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => await ExecuteAsync(async () => Ok(await _projects.GetAllAsync(CurrentUserId)));

        // GET api/projects/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => await ExecuteAsync(async () => Ok(await _projects.GetByIdAsync(id, CurrentUserId)));

        // POST api/projects
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
            => await ExecuteAsync(async () =>
            {
                var project = await _projects.CreateAsync(request, CurrentUserId);
                return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
            });

        // PUT api/projects/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectRequest request)
            => await ExecuteAsync(async () => Ok(await _projects.UpdateAsync(id, request, CurrentUserId)));

        // DELETE api/projects/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await ExecuteAsync(async () =>
            {
                await _projects.DeleteAsync(id, CurrentUserId);
                return NoContent();
            });

        // ?? Membership ????????????????????????????????????????????????????????

        // POST api/projects/{id}/members
        [HttpPost("{id:int}/members")]
        public async Task<IActionResult> AddMember(int id, [FromBody] AddMemberRequest request)
            => await ExecuteAsync(async () =>
            {
                await _projects.AddMemberAsync(id, request, CurrentUserId);
                return Ok();
            });

        // PUT api/projects/{id}/members/{userId}
        [HttpPut("{id:int}/members/{userId:int}")]
        public async Task<IActionResult> UpdateMemberRole(int id, int userId,
            [FromBody] UpdateMemberRoleRequest request)
            => await ExecuteAsync(async () =>
            {
                await _projects.UpdateMemberRoleAsync(id, userId, request, CurrentUserId);
                return NoContent();
            });

        // DELETE api/projects/{id}/members/{userId}
        [HttpDelete("{id:int}/members/{userId:int}")]
        public async Task<IActionResult> RemoveMember(int id, int userId)
            => await ExecuteAsync(async () =>
            {
                await _projects.RemoveMemberAsync(id, userId, CurrentUserId);
                return NoContent();
            });

        // ?? Audit log ?????????????????????????????????????????????????????????

        // GET api/projects/{id}/audit
        [HttpGet("{id:int}/audit")]
        public async Task<IActionResult> GetAuditLog(int id,
            [FromQuery] string? entityType = null, [FromQuery] int? entityId = null)
            => await ExecuteAsync(async () =>
                Ok(await _audit.GetLogsAsync(id, entityType, entityId)));

        // ?? Element locks ?????????????????????????????????????????????????????

        // GET api/projects/{id}/locks?entityType=Substation&entityId=5
        [HttpGet("{id:int}/locks")]
        public async Task<IActionResult> GetLockStatus(int id,
            [FromQuery] string entityType, [FromQuery] int entityId)
            => await ExecuteAsync(async () =>
                Ok(await _locks.GetStatusAsync(id, entityType, entityId)));

        // POST api/projects/{id}/locks
        [HttpPost("{id:int}/locks")]
        public async Task<IActionResult> AcquireLock(int id, [FromBody] AcquireLockRequest request)
            => await ExecuteAsync(async () =>
                Ok(await _locks.AcquireAsync(id, CurrentUserId, request)));

        // DELETE api/projects/{id}/locks?entityType=Substation&entityId=5
        [HttpDelete("{id:int}/locks")]
        public async Task<IActionResult> ReleaseLock(int id,
            [FromQuery] string entityType, [FromQuery] int entityId)
            => await ExecuteAsync(async () =>
            {
                await _locks.ReleaseAsync(id, CurrentUserId, entityType, entityId);
                return NoContent();
            });

        // DELETE api/projects/{id}/locks/all  – release all locks for the current user
        [HttpDelete("{id:int}/locks/all")]
        public async Task<IActionResult> ReleaseAllLocks(int id)
            => await ExecuteAsync(async () =>
            {
                await _locks.ReleaseAllForUserAsync(id, CurrentUserId);
                return NoContent();
            });
    }
}
