using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace G0tLib.Commands;
[ExcludeFromCodeCoverage]
public class LogCommand : Command<LogCommand.Settings>
{
    public class Settings : CommandSettings { }

    public override int Execute(CommandContext context, Settings settings)
    {
        var g0tApi = new G0tApi();
        var log = g0tApi.Log();

        foreach (var entry in log)
        {
            AnsiConsole.MarkupLine($"[yellow]⭑ Commit:[/] [bold]{entry.Hash}[/]");
            AnsiConsole.MarkupLine($"[green]✏ Message:[/] {entry.Message}");
            AnsiConsole.MarkupLine($"[blue]↩ Parent:[/] {entry.Parent}");

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("[bold]File[/]");
            table.AddColumn("[bold]Blob Hash[/]");
            foreach (var blob in entry.Blobs!)
            {
                table.AddRow(blob.Key, blob.Value);
            }
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }

        return 0;
    }
}
