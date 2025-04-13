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

    config.AddCommand<AddCommand>("add")
        .WithDescription("Stage a file for the next commit");

    config.AddCommand<StatusCommand>("status")
        .WithDescription("Show the current status of the repository");
});

return app.Run(args);