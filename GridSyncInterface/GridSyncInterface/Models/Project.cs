using System.ComponentModel.DataAnnotations;
using GridSyncInterface.Models.Auth;

namespace GridSyncInterface.Models
{
    /// <summary>
    /// A GridSync project corresponds to one SCL configuration.
    /// Multiple users can be assigned to the same project.
    /// </summary>
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int CreatedByUserId { get; set; }
        public AppUser? CreatedByUser { get; set; }

        // Optimistic concurrency – returned to clients and must be sent back on updates
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // SCL root element for this project (1-to-1)
        public int? SclId { get; set; }
        public SCL? Scl { get; set; }

        // Members
        public ICollection<ProjectMembership> Memberships { get; set; } = new List<ProjectMembership>();

        // Audit trail
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

        // Element-level locks
        public ICollection<ElementLock> ElementLocks { get; set; } = new List<ElementLock>();
    }

    /// <summary>
    /// Maps a user to a project with a project-level role.
    /// </summary>
    public class ProjectMembership
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int UserId { get; set; }
        public AppUser? User { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; } = "Viewer";   // Admin | Editor | Viewer within this project

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Audit log – every create / update / delete operation is recorded.
    /// </summary>
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int UserId { get; set; }
        public AppUser? User { get; set; }

        /// <summary>e.g. "Substation", "IED", "VoltageLevel"</summary>
        [Required, MaxLength(100)]
        public string EntityType { get; set; } = string.Empty;

        public int EntityId { get; set; }

        /// <summary>Create | Update | Delete</summary>
        [Required, MaxLength(20)]
        public string Operation { get; set; } = string.Empty;

        /// <summary>JSON snapshot of the entity before the change (null for creates).</summary>
        public string? OldValues { get; set; }

        /// <summary>JSON snapshot of the entity after the change (null for deletes).</summary>
        public string? NewValues { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Comment { get; set; }
    }

    /// <summary>
    /// Pessimistic lock on a single SCL element to prevent simultaneous edits.
    /// Locks expire automatically after <see cref="ExpiresAt"/>.
    /// </summary>
    public class ElementLock
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int UserId { get; set; }
        public AppUser? User { get; set; }

        /// <summary>e.g. "Substation", "IED"</summary>
        [Required, MaxLength(100)]
        public string EntityType { get; set; } = string.Empty;

        public int EntityId { get; set; }

        public DateTime AcquiredAt { get; set; } = DateTime.UtcNow;

        /// <summary>Default lock TTL: 5 minutes.</summary>
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
    }
}
