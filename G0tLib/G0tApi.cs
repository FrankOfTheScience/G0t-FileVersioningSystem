using G0tLib.Common;
using G0tLib.Interfaces;
using G0tLib.Commands;
using Spectre.Console;

namespace G0tLib;
public class G0tApi : IG0tApi
{
    public void Init()
    {
        Directory.CreateDirectory(G0tConstants.G0T_DIR);
        Directory.CreateDirectory(G0tConstants.G0T_OBJECTS_DIR);
        File.WriteAllText(G0tConstants.G0T_HEAD_FILE, "");
        AnsiConsole.MarkupLine("[green]✓ Initialized empty G0t repository.[/]");
    }

    public void Add(string file)
    {
        var hash = G0tHashing.HashObject(File.ReadAllText(file));
        var index = G0tIO.ReadIndex();

        if (!index.ContainsKey(file))
        {
            index[file] = hash;
            G0tIO.WriteIndex(index);
            AnsiConsole.MarkupLine($"[green]✓ Added[/] [bold]{file}[/] to staging area.");
        }
        else
        {
            AnsiConsole.MarkupLine($"[yellow]⚠ File[/] [bold]{file}[/] already staged.");
        }
    }

    public void Status()
    {
        var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                             .Where(f => !f.StartsWith(G0tConstants.G0T_DIR)).ToList();
        var index = G0tIO.ReadIndex();
        var commitHashes = new HashSet<string>();

        var commitLog = Log();
        foreach (var commit in commitLog)
        {
            commitHashes.Add(commit.Hash);
        }

        foreach (var file in files)
        {
            var hash = G0tHashing.HashObject(File.ReadAllText(file));
            if (index.ContainsKey(file))
            {
                if (index[file] != hash)
                {
                    AnsiConsole.MarkupLine($"[yellow]Modified:[/] [bold]{file}[/] - Staged but modified");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[blue]Staged:[/] [bold]{file}[/] - Ready for commit");
                }
            }
            else if (commitHashes.Contains(hash))
            {
                AnsiConsole.MarkupLine($"[green]Committed:[/] [bold]{file}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Untracked:[/] [bold]{file}[/] - Not in Git");
            }
        }
    }

    public void Commit(string message)
    {
        var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                             .Where(f => !f.StartsWith(G0tConstants.G0T_DIR)).ToList();

        var blobHashes = new List<string>();
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var hash = G0tHashing.HashObject(content);
            G0tIO.SaveObject(G0tConstants.G0T_OBJECTS_DIR, hash, content);
            blobHashes.Add($"{hash} {Path.GetFileName(file)}");
        }

        var parent = File.Exists(G0tConstants.G0T_HEAD_FILE) ? File.ReadAllText(G0tConstants.G0T_HEAD_FILE).Trim() : "";
        var commitContent = $"parent: {parent}\nmessage: {message}\nblobs:\n{string.Join("\n", blobHashes)}";

        var commitHash = G0tHashing.HashObject(commitContent);
        G0tIO.SaveObject(G0tConstants.G0T_OBJECTS_DIR, commitHash, commitContent);
        File.WriteAllText(G0tConstants.G0T_HEAD_FILE, commitHash);

        AnsiConsole.MarkupLine($"[green]✓ Committed as[/] [bold]{commitHash}[/]");
    }

    public List<CommitInfo> Log()
    {
        var result = new List<CommitInfo>();
        var current = File.Exists(G0tConstants.G0T_HEAD_FILE) ? File.ReadAllText(G0tConstants.G0T_HEAD_FILE).Trim() : "";

        while (!string.IsNullOrEmpty(current))
        {
            var content = G0tIO.ReadObject(G0tConstants.G0T_OBJECTS_DIR, current);
            var lines = content.Split('\n');
            var message = lines.FirstOrDefault(l => l.StartsWith("message:"))?.Replace("message:", "").Trim() ?? "";
            var parent = lines.FirstOrDefault(l => l.StartsWith("parent:"))?.Replace("parent:", "").Trim() ?? "";

            var blobs = lines.SkipWhile(l => !l.StartsWith("blobs:")).Skip(1)
                             .Select(l => l.Trim().Split(' '))
                             .Where(parts => parts.Length == 2)
                             .ToDictionary(parts => parts[1], parts => parts[0]);

            result.Add(new CommitInfo { Hash = current, Message = message, Parent = parent, Blobs = blobs });
            current = parent;
        }

        return result;
    }

    public void Checkout(string commitHash)
    {
        var commitContent = G0tIO.ReadObject(G0tConstants.G0T_OBJECTS_DIR, commitHash);
        var lines = commitContent.Split('\n');
        var blobs = lines.SkipWhile(l => !l.StartsWith("blobs:")).Skip(1)
                         .Select(l => l.Trim().Split(' '))
                         .Where(parts => parts.Length == 2)
                         .ToDictionary(parts => parts[1], parts => parts[0]);

        foreach (var blob in blobs)
        {
            var fileContent = G0tIO.ReadObject(G0tConstants.G0T_OBJECTS_DIR, blob.Value);
            File.WriteAllText(blob.Key, fileContent);
            AnsiConsole.MarkupLine($"[green]✓ Checked out[/] [bold]{blob.Key}[/] to commit [[{commitHash}]]");
        }
    }

    public void CreateBranch(string branchName)
    {
        var branchFile = Path.Combine(G0tConstants.G0T_DIR, "refs/heads", branchName);
        if (!Directory.Exists(Path.Combine(G0tConstants.G0T_DIR, "refs/heads")))
        {
            Directory.CreateDirectory(Path.Combine(G0tConstants.G0T_DIR, "refs/heads"));
        }

        if (File.Exists(branchFile))
        {
            AnsiConsole.MarkupLine($"[yellow]⚠ Branch[/] [bold]{branchName}[/] already exists.");
        }
        else
        {
            File.WriteAllText(branchFile, File.ReadAllText(G0tConstants.G0T_HEAD_FILE));
            AnsiConsole.MarkupLine($"[green]✓ Created new branch[/] [bold]{branchName}[/].");
        }
    }

    public void SwitchBranch(string branchName)
    {
        var branchFile = Path.Combine(G0tConstants.G0T_DIR, "refs/heads", branchName);
        if (File.Exists(branchFile))
        {
            File.WriteAllText(G0tConstants.G0T_HEAD_FILE, File.ReadAllText(branchFile));
            AnsiConsole.MarkupLine($"[green]✓ Switched to branch[/] [bold]{branchName}[/].");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]✘ Branch[/] [bold]{branchName}[/] does not exist.");
        }
    }
}

