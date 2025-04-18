﻿using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace G0tLib.Commands;
[ExcludeFromCodeCoverage]
public class CommitCommand : Command<CommitCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-m|--message <MESSAGE>")]
        [Description("Commit message")]
        public required string Message { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var g0tApi = new G0tApi();
        g0tApi.Commit(settings.Message);
        return 0;
    }
}