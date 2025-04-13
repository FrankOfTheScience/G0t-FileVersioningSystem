using Spectre.Console.Cli;

namespace G0tLib.Models;
public class StatusCommand : Command<StatusCommand.Settings>
{
    public class Settings : CommandSettings { }

    public override int Execute(CommandContext context, Settings settings)
    {
        var g0TApi = new G0tApi();
        g0TApi.Status();
        return 0;
    }
}