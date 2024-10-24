namespace RedditPlaylistGenerator.Model
{
    public record GeneratePlaylistRequestBody
    {
        public string code { get; init; }
        public string codeVerifier { get; init; }
        public string redditUrl { get; init; }
        public string playlistName { get; init; }
    }
}
