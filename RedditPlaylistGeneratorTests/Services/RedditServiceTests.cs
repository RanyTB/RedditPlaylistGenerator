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
                "Baby by Justin Bieber",
                "Baby, Justin Bieber",
                "Baby, Justin Bieber\nThis is it!",
                "Baby - Justin Bieber\nAnother song - Lady Gaga"
            };

            var extractedSongNames = RedditService.ExtractSongNames(entries);
            
            Assert.IsTrue(extractedSongNames.Count == 8, "Total count is wrong");

            var test = extractedSongNames.Where(s => s == "Baby - Justin Bieber");

            Assert.IsTrue(extractedSongNames.Where(s => s == "Baby - Justin Bieber").Count() == 7, "The count of Baby - Justin Bieber is wrong");
            Assert.IsTrue(extractedSongNames.Where(s => s == "Another song - Lady Gaga").Count() == 1, "The count of Another song - Lady Gaga is wrong");
        }
    }
}