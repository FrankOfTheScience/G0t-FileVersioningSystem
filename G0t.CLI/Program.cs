using G0tLib.Models;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.SetApplicationName("g0t");

    config.AddCommand<InitCommand>("init")
        .WithDescription("Initialize a new G0t repository");

    config.AddCommand<CommitCommand>("commit")
        .WithDescription("Commit current state")
        .WithExample(new[] { "commit", "-m", "my message" });

    config.AddCommand<LogCommand>("log")
        .WithDescription("Show commit log");
});

return app.Run(args);