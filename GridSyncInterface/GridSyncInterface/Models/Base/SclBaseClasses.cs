using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GridSyncInterface.Models.Base
{
    /// <summary>
    /// Abstract base for all SCL elements. Corresponds to tBaseElement.
    /// Provides Text, Private and a surrogate primary key for EF Core.
    /// </summary>
    public abstract class SclBaseElement
    {
        [Key]
        public int Id { get; set; }

        // Optimistic-concurrency token for conflict resolution
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Optional free-text description element (tText)
        public string? TextContent { get; set; }
        public string? TextSource { get; set; }

        // Private extensions stored as serialised XML / JSON blob
        public string? PrivateData { get; set; }
    }

    /// <summary>tUnNaming – unnamed element with optional desc attribute.</summary>
    public abstract class SclUnNaming : SclBaseElement
    {
        [MaxLength(255)]
        public string? Desc { get; set; }
    }

    /// <summary>tNaming – named element (name + optional desc).</summary>
    public abstract class SclNaming : SclBaseElement
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Desc { get; set; }
    }

    /// <summary>tIDNaming – id-named element (id + optional desc).</summary>
    public abstract class SclIDNaming : SclBaseElement
    {
        [Required]
        [MaxLength(255)]
        public string SclId { get; set; } = string.Empty;   // 'id' is a reserved EF word

        [MaxLength(255)]
        public string? Desc { get; set; }
    }
}
