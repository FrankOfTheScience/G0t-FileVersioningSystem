using G0tLib.Common;

namespace G0tLib.UnitTests;
public class G0tApiUnitTests
{
    private readonly G0tApi _g0tApi;

    public G0tApiUnitTests()
    {
        _g0tApi = new G0tApi();
    }

    [Fact]
    public void TestInit()
    {
        _g0tApi.Init();

        Assert.True(Directory.Exists(G0tConstants.G0T_DIR));
        Assert.True(Directory.Exists(G0tConstants.G0T_OBJECTS_DIR));
        Assert.True(File.Exists(G0tConstants.G0T_HEAD_FILE));

        Directory.Delete(G0tConstants.G0T_DIR, true);
    }

    [Fact]
    public void TestAdd()
    {
        _g0tApi.Init();
        var file = "test.txt";
        File.WriteAllText(file, "content");
        var hash = G0tHashing.HashObject(File.ReadAllText(file));
        var index = G0tIO.ReadIndex();

        _g0tApi.Add(file);

        index = G0tIO.ReadIndex();
        Assert.Equal(hash, index[file]);

        File.Delete(file);
        Directory.Delete(G0tConstants.G0T_DIR, true);
    }

    [Fact]
    public void TestStatus()
    {
        _g0tApi.Init();
        var file = "test.txt";
        File.WriteAllText(file, "content");
        var hash = G0tHashing.HashObject(File.ReadAllText(file));
        G0tIO.WriteIndex(new Dictionary<string, string> { { file, hash } });

        _g0tApi.Status();

        var index = G0tIO.ReadIndex();
        Assert.Equal(hash, index[file]);

        File.Delete(file);
        Directory.Delete(G0tConstants.G0T_DIR, true);
    }

    [Fact]
    public void TestCommit()
    {
        _g0tApi.Init();

        var file = "test.txt";
        File.WriteAllText(file, "content");

        _g0tApi.Commit("Initial commit");

        var headHash = File.ReadAllText(G0tConstants.G0T_HEAD_FILE).Trim();
        var commitContent = G0tIO.ReadObject(G0tConstants.G0T_OBJECTS_DIR, headHash);

        Assert.Contains("Initial commit", commitContent);
        Assert.Contains("test.txt", commitContent);

        File.Delete(file);
        Directory.Delete(G0tConstants.G0T_DIR, true);
    }

    [Fact]
    public void TestLog()
    {
        _g0tApi.Init();

        var commitHash = "commitHash1";
        var commitContent = "parent: \nmessage: Initial commit\nblobs:\nhash1 test.txt";

        Directory.CreateDirectory(G0tConstants.G0T_OBJECTS_DIR);
        File.WriteAllText(Path.Combine(G0tConstants.G0T_OBJECTS_DIR, commitHash), commitContent);
        File.WriteAllText(G0tConstants.G0T_HEAD_FILE, commitHash); 

        var result = _g0tApi.Log();

        Assert.Single(result);
        Assert.Equal("Initial commit", result[0].Message);

        Directory.Delete(G0tConstants.G0T_DIR, true);
    }

    [Fact]
    public void TestCheckout()
    {
        _g0tApi.Init();
        var commitHash = "commitHash1";
        var commitContent = "parent: \nmessage: Initial commit\nblobs:\nhash1 test.txt";
        var fileContent = "content";
        Directory.CreateDirectory(G0tConstants.G0T_OBJECTS_DIR);
        File.WriteAllText(Path.Combine(G0tConstants.G0T_OBJECTS_DIR, commitHash), commitContent);
        File.WriteAllText(Path.Combine(G0tConstants.G0T_OBJECTS_DIR, "hash1"), fileContent);

        _g0tApi.Checkout(commitHash);

        Assert.True(File.Exists("test.txt"));
        Assert.Equal(fileContent, File.ReadAllText("test.txt"));

        File.Delete("test.txt");
        Directory.Delete(G0tConstants.G0T_DIR, true);
    }

    [Fact]
    public void TestCreateBranch()
    {
        _g0tApi.Init();
        var branchName = "newBranch";
        var branchFile = Path.Combine(G0tConstants.G0T_DIR, "refs/heads", branchName);
        Directory.CreateDirectory(Path.Combine(G0tConstants.G0T_DIR, "refs/heads"));

        _g0tApi.CreateBranch(branchName);

        Assert.True(File.Exists(branchFile));

        Directory.Delete(G0tConstants.G0T_DIR, true);
    }

    [Fact]
    public void TestSwitchBranch()
    {
        _g0tApi.Init();
        var branchName = "newBranch";
        var branchFile = Path.Combine(G0tConstants.G0T_DIR, "refs/heads", branchName);
        Directory.CreateDirectory(Path.Combine(G0tConstants.G0T_DIR, "refs/heads"));
        File.WriteAllText(branchFile, "commitHash1");

        _g0tApi.SwitchBranch(branchName);

        Assert.Equal("commitHash1", File.ReadAllText(G0tConstants.G0T_HEAD_FILE));

        Directory.Delete(G0tConstants.G0T_DIR, true);
    }
}
