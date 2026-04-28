using GridSyncInterface.Parser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GridSyncInterface.Controllers
{
    /// <summary>
    /// Exposes endpoints that accept SCL file uploads (*.icd, *.iid, *.cid, *.scd),
    /// parse them into the object model and persist the result in the database.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class SclParserController : ApiBaseController
    {
        private static readonly HashSet<string> _allowedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".icd", ".iid", ".cid", ".scd" };

        private readonly SclParser _parser;

        public SclParserController(SclParser parser)
        {
            _parser = parser;
        }

        /// <summary>
        /// Upload and import a single SCL file.
        /// Supported file types: .icd, .iid, .cid, .scd
        /// </summary>
        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(SclImportResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SclImportResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Import(IFormFile file, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new SclImportResponse { Success = false, ErrorMessage = "No file provided." });

            var ext = Path.GetExtension(file.FileName);
            if (!_allowedExtensions.Contains(ext))
                return BadRequest(new SclImportResponse
                {
                    Success = false,
                    ErrorMessage = $"Unsupported file extension '{ext}'. Allowed: .icd, .iid, .cid, .scd"
                });

            await using var stream = file.OpenReadStream();
            var result = await _parser.ParseStreamAsync(stream, ct);

            if (!result.Success)
                return UnprocessableEntity(new SclImportResponse
                {
                    Success = false,
                    ErrorMessage = result.ErrorMessage,
                    Warnings = result.Warnings
                });

            return Ok(new SclImportResponse
            {
                Success = true,
                SclId = result.SclId,
                FileName = file.FileName,
                Warnings = result.Warnings
            });
        }
    }

    /// <summary>Response DTO for the import endpoint.</summary>
    public class SclImportResponse
    {
        public bool Success { get; set; }
        public int? SclId { get; set; }
        public string? FileName { get; set; }
        public string? ErrorMessage { get; set; }
        public IReadOnlyList<string> Warnings { get; set; } = [];
    }
}
