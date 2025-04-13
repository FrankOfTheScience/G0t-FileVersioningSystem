using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace G0tLib.Commands;
[ExcludeFromCodeCoverage]
public class AddCommand : Command<AddCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "file")]
        [Description("File to stage")]
        public required string FileName { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var g0tApi = new G0tApi();
        g0tApi.Add(settings.FileName);
        return 0;
    }
}
