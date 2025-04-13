using G0tLib.Common;

namespace G0tLib.UnitTests.Common;
public class G0tHashingUnitTests
{
    [Fact]
    public void TestHashObject()
    {
        var content = "Hello, world!";
        var hash = G0tHashing.HashObject(content);

        Assert.Equal("315f5bdb76d078c43b8ac0064e4a0164612b1fce77c869345bfc94c75894edd3", hash);
    }

    [Fact]
    public void TestHashObject_EmptyString()
    {
        var content = "";
        var hash = G0tHashing.HashObject(content);

        Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", hash);
    }

    [Fact]
    public void TestHashObject_SpecialCharacters()
    {
        var content = "!@#$%^&*()_+";
        var hash = G0tHashing.HashObject(content);

        Assert.Equal("36d3e1bc65f8b67935ae60f542abef3e55c5bbbd547854966400cc4f022566cb", hash);
    }
}
