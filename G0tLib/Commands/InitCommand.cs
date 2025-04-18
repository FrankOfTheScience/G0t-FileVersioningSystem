﻿using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace G0tLib.Commands;
[ExcludeFromCodeCoverage]
public class InitCommand : Command<InitCommand.Settings>
{
    public class Settings : CommandSettings { }

    public override int Execute(CommandContext context, Settings settings)
    {
        var g0tApi = new G0tApi();
        g0tApi.Init();
        return 0;
    }
}