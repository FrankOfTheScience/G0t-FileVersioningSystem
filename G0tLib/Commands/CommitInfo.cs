using System.Diagnostics.CodeAnalysis;

namespace G0tLib.Commands;
[ExcludeFromCodeCoverage]
public class CommitInfo
{
    public required string Hash { get; set; }
    public required string Message { get; set; }
    public required string Parent { get; set; }
    public Dictionary<string, string>? Blobs { get; set; }
}
