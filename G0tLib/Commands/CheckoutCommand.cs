﻿using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace G0tLib.Commands;
[ExcludeFromCodeCoverage]
public class CheckoutCommand : Command<CheckoutCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<COMMIT_HASH>")]
        [Description("Commit hash to checkout")]
        public required string CommitHash { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var g0tApi = new G0tApi();
        g0tApi.Checkout(settings.CommitHash);
        return 0;
    }
}
