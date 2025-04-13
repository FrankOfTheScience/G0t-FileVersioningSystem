using Spectre.Console.Cli;
using System.ComponentModel;

namespace G0tLib.Models;
public class BranchCommand : Command<BranchCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-c|--create <BRANCH_NAME>")]
        [Description("Create a new branch")]
        public required string BranchToCreate { get; set; }

        [CommandOption("-s|--switch <BRANCH_NAME>")]
        [Description("Switch to an existing branch")]
        public required string BranchToSwitch { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var g0tApi = new G0tApi();

        if (!string.IsNullOrEmpty(settings.BranchToCreate))
        {
            g0tApi.CreateBranch(settings.BranchToCreate);
        }
        else if (!string.IsNullOrEmpty(settings.BranchToSwitch))
        {
            g0tApi.SwitchBranch(settings.BranchToSwitch);
        }

        return 0;
    }
}