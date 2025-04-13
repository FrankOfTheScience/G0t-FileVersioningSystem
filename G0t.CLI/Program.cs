using G0tLib;

var command = args[0];
var miniGit = new G0tApi();

switch (command)
{
    case "init":
        miniGit.Init();
        break;

    case "commit":
        var message = args.Length > 1 ? string.Join(" ", args.Skip(1)) : "No message";
        miniGit.Commit(message);
        break;

    case "log":
        var log = miniGit.Log();
        foreach (var entry in log)
        {
            Console.WriteLine($"\nCommit: {entry.Hash}\nMessage: {entry.Message}\nParent: {entry.Parent}");
            foreach (var blob in entry.Blobs!)
            {
                Console.WriteLine($"  - {blob.Key}: {blob.Value}");
            }
        }
        break;

    default:
        Console.WriteLine($"Unknown command: {command}");
        break;
}