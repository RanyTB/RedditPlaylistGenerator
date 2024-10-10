namespace RedditPlaylistGenerator.Services.Tests
{
    [TestClass()]
    public class RedditServiceTests
    {
        [TestMethod()]
        public void ExtractSongNamesTest()
        {
            var entries = new List<string>
            {
                "Baby - Justin Bieber",
                "Baby- Justin Bieber",
                "Baby-Justin Bieber",
                "Baby by Justin bieber",
                "Baby, Justin Bieber",
                "Baby, Justin Bieber\nThis is it!",
                "Baby - Justin Bieber\nAnother song - Lady Gaga"
            };

            var extractedSongNames = RedditService.ExtractSongNames(entries);

            Assert.IsTrue(extractedSongNames.Count == 8);
        }
    }
}