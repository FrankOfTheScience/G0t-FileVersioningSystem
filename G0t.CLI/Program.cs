using G0tLib.Common;
using G0tLib.Models;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.SetApplicationName(G0tConstants.GOT_APPLICATION_NAME);

    config.AddCommand<InitCommand>(G0tConstants.GOT_INIT_COMMAND)
        .WithDescription("Initialize a new G0t repository");

    config.AddCommand<CommitCommand>(G0tConstants.GOT_COMMIT_COMMAND)
        .WithDescription("Commit current state")
        .WithExample([G0tConstants.GOT_COMMIT_COMMAND, "-m", "my message"]);

    config.AddCommand<LogCommand>(G0tConstants.GOT_LOG_COMMAND)
        .WithDescription("Show commit log");

    config.AddCommand<AddCommand>(G0tConstants.GOT_ADD_COMMAND)
        .WithDescription("Stage a file for the next commit");

    config.AddCommand<StatusCommand>(G0tConstants.GOT_STATUS_COMMAND)
        .WithDescription("Show the current status of the repository");
});

return app.Run(args);