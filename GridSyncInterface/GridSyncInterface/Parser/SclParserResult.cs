namespace GridSyncInterface.Parser
{
    /// <summary>
    /// Returned by <see cref="SclParser"/> after a parse/import operation.
    /// </summary>
    public class SclParserResult
    {
        /// <summary>Whether the parse and database save succeeded without fatal errors.</summary>
        public bool Success { get; init; }

        /// <summary>Database primary-key of the persisted <c>SCL</c> root row, or <c>null</c> on failure.</summary>
        public int? SclId { get; init; }

        /// <summary>Non-fatal warnings collected during parsing (e.g. unknown attributes).</summary>
        public IReadOnlyList<string> Warnings { get; init; } = [];

        /// <summary>Error message when <see cref="Success"/> is <c>false</c>.</summary>
        public string? ErrorMessage { get; init; }

        public static SclParserResult Ok(int sclId, IEnumerable<string> warnings) =>
            new() { Success = true, SclId = sclId, Warnings = warnings.ToList() };

        public static SclParserResult Fail(string message) =>
            new() { Success = false, ErrorMessage = message };
    }
}
