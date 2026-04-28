using System.Text.Json;
using GridSyncInterface.Data;
using GridSyncInterface.DTOs.Scl;
using GridSyncInterface.Models;
using GridSyncInterface.Models.Auth;
using GridSyncInterface.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace GridSyncInterface.Services
{
    /// <summary>
    /// Central conflict-resolution service for SCL element CRUD.
    ///
    /// Strategy
    /// ?????????
    /// 1.  Optimistic concurrency  – every model row carries a SQL Server rowversion
    ///     (byte[] RowVersion).  The client sends back the base-64 token it received on
    ///     the last GET.  If the stored token differs a 409 is returned with a diff of
    ///     the conflicting fields so the UI can show a three-way merge.
    ///
    /// 2.  Pessimistic locking  – before starting a long edit a user calls
    ///     POST /api/projects/{id}/locks to acquire an element-level lock.
    ///     Any write from a different user to the same element while the lock is held
    ///     also returns 409 (with the lock holder's name and expiry).
    ///
    /// 3.  Last-writer-wins fallback  – callers may explicitly choose this strategy by
    ///     sending RowVersion = null, which skips the concurrency check (useful for
    ///     automated import tools).
    /// </summary>
    public interface IConflictResolutionService
    {
        /// <summary>
        /// Validates that a write operation is safe.
        /// Throws <see cref="ConcurrencyConflictException"/> on version mismatch.
        /// Throws <see cref="ElementLockedException"/> if another user holds the lock.
        /// </summary>
        Task CheckAsync(int projectId, int userId, string entityType, int entityId,
                        string? incomingRowVersion, SclBaseElement entity);

        /// <summary>Produces a field-level diff for UI conflict presentation.</summary>
        ConflictDiff BuildDiff(string? clientJson, string? serverJson);
    }

    public class ConflictResolutionService : IConflictResolutionService
    {
        private readonly ILockService _lockService;

        public ConflictResolutionService(ILockService lockService)
            => _lockService = lockService;

        public async Task CheckAsync(int projectId, int userId, string entityType, int entityId,
                                     string? incomingRowVersion, SclBaseElement entity)
        {
            // 1 ?? Pessimistic lock check ??????????????????????????????????????
            if (await _lockService.IsLockedByOtherAsync(projectId, userId, entityType, entityId))
            {
                var status = await _lockService.GetStatusAsync(projectId, entityType, entityId);
                throw new ElementLockedException(
                    $"'{entityType}' (id={entityId}) is locked by '{status.LockedBy}' until {status.ExpiresAt:u}.");
            }

            // 2 ?? Optimistic concurrency check ????????????????????????????????
            if (incomingRowVersion != null && entity.RowVersion != null)
            {
                byte[] clientVersion;
                try   { clientVersion = Convert.FromBase64String(incomingRowVersion); }
                catch { throw new ConcurrencyConflictException("RowVersion is not valid base-64."); }

                if (!clientVersion.SequenceEqual(entity.RowVersion))
                    throw new ConcurrencyConflictException(
                        $"Concurrency conflict on '{entityType}' (id={entityId}). " +
                        "Another user has modified this element. Refresh and merge your changes.");
            }
            // If incomingRowVersion is null ? last-writer-wins (no check)
        }

        public ConflictDiff BuildDiff(string? clientJson, string? serverJson)
        {
            var diff  = new ConflictDiff();
            if (clientJson == null || serverJson == null) return diff;

            using var clientDoc = JsonDocument.Parse(clientJson);
            using var serverDoc = JsonDocument.Parse(serverJson);

            foreach (var serverProp in serverDoc.RootElement.EnumerateObject())
            {
                if (!clientDoc.RootElement.TryGetProperty(serverProp.Name, out var clientProp))
                {
                    diff.AddedOnServer.Add(serverProp.Name, serverProp.Value.ToString());
                    continue;
                }

                var clientVal = clientProp.ToString();
                var serverVal = serverProp.Value.ToString();
                if (clientVal != serverVal)
                {
                    diff.ConflictingFields.Add(serverProp.Name, new FieldConflict
                    {
                        ClientValue = clientVal,
                        ServerValue = serverVal
                    });
                }
            }
            return diff;
        }
    }

    // ?? Conflict domain types ????????????????????????????????????????????????

    public class ConcurrencyConflictException : Exception
    {
        public ConcurrencyConflictException(string message) : base(message) { }
    }

    public class ElementLockedException : Exception
    {
        public ElementLockedException(string message) : base(message) { }
    }

    public class ConflictDiff
    {
        public Dictionary<string, FieldConflict> ConflictingFields { get; } = new();
        public Dictionary<string, string>        AddedOnServer     { get; } = new();
    }

    public class FieldConflict
    {
        public string ClientValue { get; set; } = string.Empty;
        public string ServerValue { get; set; } = string.Empty;
    }
}
