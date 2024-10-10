namespace RedditPlaylistGenerator.Configuration
{
    public class RedditOptions
    {
        public const string Key = "Reddit";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserAgent { get; set; }
    }
}
