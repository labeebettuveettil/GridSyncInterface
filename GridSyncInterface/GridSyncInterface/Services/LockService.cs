using GridSyncInterface.Data;
using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Models;
using Microsoft.EntityFrameworkCore;

namespace GridSyncInterface.Services
{
    /// <summary>
    /// Handles all element-level pessimistic locking.
    /// A user acquires a lock before editing; the lock expires after a TTL.
    /// Other users receive 409 Conflict if they try to edit a locked element.
    /// </summary>
    public interface ILockService
    {
        Task<LockStatusDto> GetStatusAsync(int projectId, string entityType, int entityId);
        Task<LockStatusDto> AcquireAsync(int projectId, int userId, AcquireLockRequest request);
        Task ReleaseAsync(int projectId, int userId, string entityType, int entityId);
        Task ReleaseAllForUserAsync(int projectId, int userId);
        Task<bool> IsLockedByOtherAsync(int projectId, int userId, string entityType, int entityId);
        Task PurgeExpiredAsync();
    }

    public class LockService : ILockService
    {
        private readonly SclDbContext _db;

        public LockService(SclDbContext db) => _db = db;

        public async Task<LockStatusDto> GetStatusAsync(int projectId, string entityType, int entityId)
        {
            await PurgeExpiredAsync();
            var lck = await _db.ElementLocks
                .Include(l => l.User)
                .FirstOrDefaultAsync(l =>
                    l.ProjectId  == projectId &&
                    l.EntityType == entityType &&
                    l.EntityId   == entityId);

            return lck == null
                ? new LockStatusDto { IsLocked = false }
                : new LockStatusDto { IsLocked = true, LockedBy = lck.User?.Username, ExpiresAt = lck.ExpiresAt };
        }

        public async Task<LockStatusDto> AcquireAsync(int projectId, int userId, AcquireLockRequest request)
        {
            await PurgeExpiredAsync();

            // Check if someone else already holds the lock
            var existing = await _db.ElementLocks
                .Include(l => l.User)
                .FirstOrDefaultAsync(l =>
                    l.ProjectId  == projectId &&
                    l.EntityType == request.EntityType &&
                    l.EntityId   == request.EntityId);

            if (existing != null && existing.UserId != userId)
                throw new InvalidOperationException(
                    $"Element is locked by '{existing.User?.Username}' until {existing.ExpiresAt:u}.");

            var durationMins = Math.Clamp(request.DurationMinutes, 1, 60);

            if (existing != null)
            {
                // Renew own lock
                existing.ExpiresAt = DateTime.UtcNow.AddMinutes(durationMins);
            }
            else
            {
                _db.ElementLocks.Add(new ElementLock
                {
                    ProjectId  = projectId,
                    UserId     = userId,
                    EntityType = request.EntityType,
                    EntityId   = request.EntityId,
                    AcquiredAt = DateTime.UtcNow,
                    ExpiresAt  = DateTime.UtcNow.AddMinutes(durationMins)
                });
            }

            await _db.SaveChangesAsync();
            var user = await _db.Users.FindAsync(userId);
            return new LockStatusDto { IsLocked = true, LockedBy = user?.Username, ExpiresAt = DateTime.UtcNow.AddMinutes(durationMins) };
        }

        public async Task ReleaseAsync(int projectId, int userId, string entityType, int entityId)
        {
            var lck = await _db.ElementLocks
                .FirstOrDefaultAsync(l =>
                    l.ProjectId  == projectId &&
                    l.UserId     == userId &&
                    l.EntityType == entityType &&
                    l.EntityId   == entityId);

            if (lck != null)
            {
                _db.ElementLocks.Remove(lck);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ReleaseAllForUserAsync(int projectId, int userId)
        {
            var locks = await _db.ElementLocks
                .Where(l => l.ProjectId == projectId && l.UserId == userId)
                .ToListAsync();
            _db.ElementLocks.RemoveRange(locks);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsLockedByOtherAsync(int projectId, int userId, string entityType, int entityId)
        {
            await PurgeExpiredAsync();
            return await _db.ElementLocks.AnyAsync(l =>
                l.ProjectId  == projectId &&
                l.UserId     != userId &&
                l.EntityType == entityType &&
                l.EntityId   == entityId);
        }

        public async Task PurgeExpiredAsync()
        {
            var expired = await _db.ElementLocks
                .Where(l => l.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();
            if (expired.Count > 0)
            {
                _db.ElementLocks.RemoveRange(expired);
                await _db.SaveChangesAsync();
            }
        }
    }
}
