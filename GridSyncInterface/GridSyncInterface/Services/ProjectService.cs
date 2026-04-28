using GridSyncInterface.Data;
using GridSyncInterface.DTOs.Projects;
using GridSyncInterface.Models;
using GridSyncInterface.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace GridSyncInterface.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync(int requestingUserId);
        Task<ProjectDto> GetByIdAsync(int projectId, int requestingUserId);
        Task<ProjectDto> CreateAsync(CreateProjectRequest request, int userId);
        Task<ProjectDto> UpdateAsync(int projectId, UpdateProjectRequest request, int userId);
        Task DeleteAsync(int projectId, int userId);
        Task AddMemberAsync(int projectId, AddMemberRequest request, int requestingUserId);
        Task UpdateMemberRoleAsync(int projectId, int memberId, UpdateMemberRoleRequest request, int requestingUserId);
        Task RemoveMemberAsync(int projectId, int memberId, int requestingUserId);
    }

    public class ProjectService : IProjectService
    {
        private readonly SclDbContext _db;
        private readonly IAuditService _audit;

        public ProjectService(SclDbContext db, IAuditService audit)
        {
            _db    = db;
            _audit = audit;
        }

        // ?? Query ????????????????????????????????????????????????????????????
        public async Task<IEnumerable<ProjectDto>> GetAllAsync(int requestingUserId)
        {
            // Admins see all projects; others see only those they're members of
            var user = await _db.Users.FindAsync(requestingUserId)
                ?? throw new KeyNotFoundException("User not found.");

            var query = _db.Projects
                .Include(p => p.CreatedByUser)
                .Include(p => p.Memberships).ThenInclude(m => m.User)
                .AsQueryable();

            if (user.Role != UserRoles.Admin)
                query = query.Where(p => p.Memberships.Any(m => m.UserId == requestingUserId));

            var projects = await query.ToListAsync();
            return projects.Select(MapDto);
        }

        public async Task<ProjectDto> GetByIdAsync(int projectId, int requestingUserId)
        {
            var project = await GetProjectOrThrowAsync(projectId);
            EnsureAccess(project, requestingUserId);
            return MapDto(project);
        }

        // ?? Create ???????????????????????????????????????????????????????????
        public async Task<ProjectDto> CreateAsync(CreateProjectRequest request, int userId)
        {
            // Every new project gets a blank SCL root
            var scl = new SCL { Version = "2007", Revision = "B", Release = 4 };
            _db.SCLs.Add(scl);
            await _db.SaveChangesAsync();   // populate scl.Id

            var project = new Project
            {
                Name            = request.Name,
                Description     = request.Description,
                CreatedByUserId = userId,
                UpdatedAt       = DateTime.UtcNow,
                SclId           = scl.Id
            };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

            // Creator automatically becomes Admin member
            _db.ProjectMemberships.Add(new ProjectMembership
            {
                ProjectId  = project.Id,
                UserId     = userId,
                Role       = UserRoles.Admin
            });
            await _db.SaveChangesAsync();

            await _audit.LogAsync(project.Id, userId, "Project", project.Id, "Create", null,
                System.Text.Json.JsonSerializer.Serialize(new { project.Name, project.Description }));

            return MapDto(await GetProjectOrThrowAsync(project.Id));
        }

        // ?? Update ???????????????????????????????????????????????????????????
        public async Task<ProjectDto> UpdateAsync(int projectId, UpdateProjectRequest request, int userId)
        {
            var project = await GetProjectOrThrowAsync(projectId);
            EnsureEditorAccess(project, userId);

            // Optimistic concurrency check
            var incoming = Convert.FromBase64String(request.RowVersion);
            if (!project.RowVersion!.SequenceEqual(incoming))
                throw new DbUpdateConcurrencyException(
                    "The project was modified by another user. Please refresh and try again.");

            var oldJson = System.Text.Json.JsonSerializer.Serialize(new { project.Name, project.Description });

            project.Name        = request.Name;
            project.Description = request.Description;
            project.UpdatedAt   = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            await _audit.LogAsync(projectId, userId, "Project", projectId, "Update", oldJson,
                System.Text.Json.JsonSerializer.Serialize(new { project.Name, project.Description }));

            return MapDto(await GetProjectOrThrowAsync(projectId));
        }

        // ?? Delete ???????????????????????????????????????????????????????????
        public async Task DeleteAsync(int projectId, int userId)
        {
            var project = await GetProjectOrThrowAsync(projectId);
            EnsureAdminAccess(project, userId);

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }

        // ?? Membership ???????????????????????????????????????????????????????
        public async Task AddMemberAsync(int projectId, AddMemberRequest request, int requestingUserId)
        {
            var project = await GetProjectOrThrowAsync(projectId);
            EnsureAdminAccess(project, requestingUserId);

            if (!await _db.Users.AnyAsync(u => u.Id == request.UserId))
                throw new KeyNotFoundException($"User {request.UserId} not found.");

            if (project.Memberships.Any(m => m.UserId == request.UserId))
                throw new InvalidOperationException("User is already a member of this project.");

            _db.ProjectMemberships.Add(new ProjectMembership
            {
                ProjectId = projectId,
                UserId    = request.UserId,
                Role      = request.Role
            });
            await _db.SaveChangesAsync();
        }

        public async Task UpdateMemberRoleAsync(int projectId, int memberId, UpdateMemberRoleRequest request, int requestingUserId)
        {
            var project    = await GetProjectOrThrowAsync(projectId);
            EnsureAdminAccess(project, requestingUserId);
            var membership = project.Memberships.FirstOrDefault(m => m.UserId == memberId)
                ?? throw new KeyNotFoundException("Membership not found.");
            membership.Role = request.Role;
            await _db.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(int projectId, int memberId, int requestingUserId)
        {
            var project    = await GetProjectOrThrowAsync(projectId);
            EnsureAdminAccess(project, requestingUserId);
            var membership = project.Memberships.FirstOrDefault(m => m.UserId == memberId)
                ?? throw new KeyNotFoundException("Membership not found.");
            _db.ProjectMemberships.Remove(membership);
            await _db.SaveChangesAsync();
        }

        // ?? Helpers ??????????????????????????????????????????????????????????
        private async Task<Project> GetProjectOrThrowAsync(int projectId) =>
            await _db.Projects
                .Include(p => p.CreatedByUser)
                .Include(p => p.Memberships).ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new KeyNotFoundException($"Project {projectId} not found.");

        private static void EnsureAccess(Project project, int userId)
        {
            // Viewer / Editor / Admin all can read
            if (!project.Memberships.Any(m => m.UserId == userId))
                throw new UnauthorizedAccessException("You do not have access to this project.");
        }

        private static void EnsureEditorAccess(Project project, int userId)
        {
            var m = project.Memberships.FirstOrDefault(m => m.UserId == userId)
                ?? throw new UnauthorizedAccessException("You do not have access to this project.");
            if (m.Role == UserRoles.Viewer)
                throw new UnauthorizedAccessException("You need Editor or Admin rights to modify this project.");
        }

        private static void EnsureAdminAccess(Project project, int userId)
        {
            var m = project.Memberships.FirstOrDefault(m => m.UserId == userId)
                ?? throw new UnauthorizedAccessException("You do not have access to this project.");
            if (m.Role != UserRoles.Admin)
                throw new UnauthorizedAccessException("You need Admin rights for this operation.");
        }

        private static ProjectDto MapDto(Project p) => new()
        {
            Id          = p.Id,
            Name        = p.Name,
            Description = p.Description,
            CreatedAt   = p.CreatedAt,
            UpdatedAt   = p.UpdatedAt,
            CreatedBy   = p.CreatedByUser?.Username ?? string.Empty,
            SclId       = p.SclId,
            RowVersion  = p.RowVersion != null ? Convert.ToBase64String(p.RowVersion) : string.Empty,
            Members     = p.Memberships.Select(m => new ProjectMemberDto
            {
                UserId   = m.UserId,
                Username = m.User?.Username ?? string.Empty,
                Role     = m.Role
            })
        };
    }
}
