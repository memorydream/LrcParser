using Xunit;
using LrcParser;

namespace LrcParser.Tests
{
    public class LyricParseSortTests
    {
        [Fact]
        public void SortLyrics()
        {
            var result = LyricParser.ParseSingleLine("[02:40.32][01:54.07][00:50.22]茶饭不思 呆呆的凝望着").SortByTimestamp();
            var l1 = new string[]
            {
                result[0].ToString(),
                result[1].ToString(),
                result[2].ToString()
            };
            var l2 = new string[]
            {
                "[00:50.22]茶饭不思 呆呆的凝望着",
                "[01:54.7]茶饭不思 呆呆的凝望着",
                "[02:40.32]茶饭不思 呆呆的凝望着"
            };
            Assert.Equal(l2, l1);
        }

        [Fact]
        public void SortLyricsCompNotSort()
        {
            var result = LyricParser.ParseSingleLine("[02:40.32][01:54.07][00:50.22]茶饭不思 呆呆的凝望着");
            var l1 = new string[]
            {
                result[0].ToString(),
                result[1].ToString(),
                result[2].ToString()
            };
            var l2 = new string[]
            {
                "[00:50.22]茶饭不思 呆呆的凝望着",
                "[01:54.7]茶饭不思 呆呆的凝望着",
                "[02:40.32]茶饭不思 呆呆的凝望着"
            };
            Assert.NotEqual(l2, l1);
        }
    }
}