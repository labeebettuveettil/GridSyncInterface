using System.ComponentModel.DataAnnotations;

namespace GridSyncInterface.DTOs.Projects
{
    public class CreateProjectRequest
    {
        [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
        [MaxLength(1000)]          public string? Description { get; set; }
    }

    public class UpdateProjectRequest
    {
        [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
        [MaxLength(1000)]          public string? Description { get; set; }
        /// <summary>Concurrency token returned from the last GET – must be sent back.</summary>
        [Required]                 public string RowVersion { get; set; } = string.Empty;
    }

    public class ProjectDto
    {
        public int    Id          { get; set; }
        public string Name        { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy   { get; set; } = string.Empty;
        public int?   SclId       { get; set; }
        /// <summary>Base-64 encoded RowVersion – send back on updates.</summary>
        public string RowVersion  { get; set; } = string.Empty;
        public IEnumerable<ProjectMemberDto> Members { get; set; } = Enumerable.Empty<ProjectMemberDto>();
    }

    public class ProjectMemberDto
    {
        public int    UserId   { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role     { get; set; } = string.Empty;
    }

    public class AddMemberRequest
    {
        [Required] public int    UserId { get; set; }
        [Required, MaxLength(50)] public string Role { get; set; } = "Viewer";
    }

    public class UpdateMemberRoleRequest
    {
        [Required, MaxLength(50)] public string Role { get; set; } = string.Empty;
    }
}
