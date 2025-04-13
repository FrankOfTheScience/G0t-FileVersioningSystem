using G0tLib.Common;

namespace G0tLib.UnitTests.Common;
public class G0tIOUnitTests
{
    [Fact]
    public void TestReadIndex_FileExists()
    {
        var indexPath = ".minigit/index";
        Directory.CreateDirectory(".minigit");
        File.WriteAllLines(indexPath, new[] { "hash1 file1", "hash2 file2" });

        var result = G0tIO.ReadIndex();

        Assert.Equal(2, result.Count);
        Assert.Equal("hash1", result["file1"]);
        Assert.Equal("hash2", result["file2"]);

        File.Delete(indexPath);
        Directory.Delete(".minigit");
    }

    [Fact]
    public void TestReadIndex_FileNotExists()
    {
        var result = G0tIO.ReadIndex();

        Assert.Empty(result);
    }

    [Fact]
    public void TestWriteIndex()
    {
        var indexPath = ".minigit/index";
        Directory.CreateDirectory(".minigit");
        var index = new Dictionary<string, string>
        {
            { "file1", "hash1" },
            { "file2", "hash2" }
        };

        G0tIO.WriteIndex(index);

        var lines = File.ReadAllLines(indexPath);
        Assert.Equal(2, lines.Length);
        Assert.Contains("hash1 file1", lines);
        Assert.Contains("hash2 file2", lines);

        File.Delete(indexPath);
        Directory.Delete(".minigit");
    }

    [Fact]
    public void TestSaveObject()
    {
        var objectsDir = ".minigit/objects";
        Directory.CreateDirectory(objectsDir);
        var hash = "hash1";
        var content = "content1";

        G0tIO.SaveObject(objectsDir, hash, content);

        var filePath = Path.Combine(objectsDir, hash);
        Assert.True(File.Exists(filePath));
        Assert.Equal(content, File.ReadAllText(filePath));

        File.Delete(filePath);
        Directory.Delete(objectsDir);
    }

    [Fact]
    public void TestReadObject()
    {
        var objectsDir = ".minigit/objects";
        Directory.CreateDirectory(objectsDir);
        var hash = "hash1";
        var content = "content1";
        var filePath = Path.Combine(objectsDir, hash);
        File.WriteAllText(filePath, content);

        var result = G0tIO.ReadObject(objectsDir, hash);

        Assert.Equal(content, result);

        File.Delete(filePath);
        Directory.Delete(objectsDir);
    }
}
