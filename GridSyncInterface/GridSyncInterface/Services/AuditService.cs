using GridSyncInterface.Data;
using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Models;
using Microsoft.EntityFrameworkCore;

namespace GridSyncInterface.Services
{
    /// <summary>
    /// Writes audit log entries for every CRUD operation on SCL elements.
    /// </summary>
    public interface IAuditService
    {
        Task LogAsync(int projectId, int userId, string entityType, int entityId,
                      string operation, string? oldValues, string? newValues, string? comment = null);
        Task<IEnumerable<AuditLogDto>> GetLogsAsync(int projectId, string? entityType = null, int? entityId = null);
    }

    public class AuditService : IAuditService
    {
        private readonly SclDbContext _db;

        public AuditService(SclDbContext db) => _db = db;

        public async Task LogAsync(int projectId, int userId, string entityType, int entityId,
                                   string operation, string? oldValues, string? newValues, string? comment = null)
        {
            _db.AuditLogs.Add(new AuditLog
            {
                ProjectId  = projectId,
                UserId     = userId,
                EntityType = entityType,
                EntityId   = entityId,
                Operation  = operation,
                OldValues  = oldValues,
                NewValues  = newValues,
                Comment    = comment,
                Timestamp  = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLogDto>> GetLogsAsync(int projectId,
                                                                  string? entityType = null,
                                                                  int? entityId = null)
        {
            var query = _db.AuditLogs
                .Include(a => a.User)
                .Where(a => a.ProjectId == projectId);

            if (entityType != null) query = query.Where(a => a.EntityType == entityType);
            if (entityId.HasValue)  query = query.Where(a => a.EntityId   == entityId.Value);

            var logs = await query.OrderByDescending(a => a.Timestamp).ToListAsync();

            return logs.Select(a => new AuditLogDto
            {
                Id         = a.Id,
                EntityType = a.EntityType,
                EntityId   = a.EntityId,
                Operation  = a.Operation,
                Username   = a.User?.Username ?? string.Empty,
                Timestamp  = a.Timestamp,
                OldValues  = a.OldValues,
                NewValues  = a.NewValues,
                Comment    = a.Comment
            });
        }
    }
}
