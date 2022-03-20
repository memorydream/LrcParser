using Xunit;
using LrcParser;

namespace LrcParser.Tests;

public class LyricParseSingleLineTests
{
    [Fact]
    public void ParseSingeTimestamp()
    {
        var result = new LyricParser().ParseSingleLine("[03:03.67]最近少一个人好寂寞")[0];
        Assert.Equal("[03:03.67]最近少一个人好寂寞", result.ToString());
    }

    [Fact]
    public void ParseMultiTimestamp()
    {
        var result = new LyricParser().ParseSingleLine("[02:40.32][01:54.07][00:50.22]茶饭不思 呆呆的凝望着");
        var l1 = new string[] 
        {
            result[0].ToString(),
            result[1].ToString(),
            result[2].ToString()
        };
        var l2 = new string[]
        {
            "[02:40.32]茶饭不思 呆呆的凝望着",
            "[01:54.7]茶饭不思 呆呆的凝望着",
            "[00:50.22]茶饭不思 呆呆的凝望着"
        };
        Assert.Equal(l2, l1);
    }

    [Fact]
    public void ParseWithoutTimestamp()
    {
        Assert.Null(new LyricParser().ParseSingleLine("茶饭不思 呆呆的凝望着"));
    }

    [Fact]
    public void ParseWithOutContent()
    {
        var result = new LyricParser().ParseSingleLine("[03:03.67]")[0];
        Assert.Equal("[03:03.67]", result.ToString());
    }

    [Fact]
    public void ParseFakeTimestampNumber()
    {
        var result = new LyricParser().ParseSingleLine("[03:03.67][123]qqaa")[0];
        Assert.Equal("[123]qqaa", result.Content);
    }

    [Fact]
    public void ParseFakeTimestampString()
    {
        var result = new LyricParser().ParseSingleLine("[03:03.67][aa:bb.ccc]")[0];
        Assert.Equal("[aa:bb.ccc]", result.Content);
    }

    [Fact]
    public void ParseEmpty()
    {
        Assert.Null(new LyricParser().ParseSingleLine(string.Empty));
    }

    [Fact]
    public void ParseNull()
    {
        Assert.Null(new LyricParser().ParseSingleLine(null));
    }

    [Fact]
    public void ParseWithoutMilliseconds()
    {
        Assert.Equal("[02:40.0]最近少一个人好寂寞", new LyricParser().ParseSingleLine("[02:40]最近少一个人好寂寞")[0].ToString());
    }
}