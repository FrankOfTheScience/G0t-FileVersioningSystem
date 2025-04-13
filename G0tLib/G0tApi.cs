using System.Security.Cryptography;
using System.Text;
using G0tLib.Interfaces;
using G0tLib.Models;

namespace G0tLib;
public class G0tApi : IG0tApi
{
    private const string GitDir = ".g0t";
    private const string ObjectsDir = ".g0t/objects";
    private const string HeadFile = ".g0t/HEAD";

    public void Init()
    {
        Directory.CreateDirectory(GitDir);
        Directory.CreateDirectory(ObjectsDir);
        File.WriteAllText(HeadFile, "");
        Console.WriteLine("Initialized empty G0t repository.");
    }

    public void Commit(string message)
    {
        var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                             .Where(f => !f.StartsWith(GitDir)).ToList();

        var blobHashes = new List<string>();
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var hash = HashObject(content);
            SaveObject(hash, content);
            blobHashes.Add($"{hash} {Path.GetFileName(file)}");
        }

        var parent = File.Exists(HeadFile) ? File.ReadAllText(HeadFile).Trim() : "";
        var commitContent = $"parent: {parent}\nmessage: {message}\nblobs:\n{string.Join("\n", blobHashes)}";

        var commitHash = HashObject(commitContent);
        SaveObject(commitHash, commitContent);
        File.WriteAllText(HeadFile, commitHash);

        Console.WriteLine($"Committed as {commitHash}");
    }

    public List<CommitInfo> Log()
    {
        var result = new List<CommitInfo>();
        var current = File.Exists(HeadFile) ? File.ReadAllText(HeadFile).Trim() : "";

        while (!string.IsNullOrEmpty(current))
        {
            var content = ReadObject(current);
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

    private string HashObject(string content)
    {
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    private void SaveObject(string hash, string content)
    {
        File.WriteAllText(Path.Combine(ObjectsDir, hash), content);
    }

    private string ReadObject(string hash)
    {
        return File.ReadAllText(Path.Combine(ObjectsDir, hash));
    }
}

