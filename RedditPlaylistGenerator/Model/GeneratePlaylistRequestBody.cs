namespace RedditPlaylistGenerator.Model
{
    public record GeneratePlaylistRequestBody
    {
        public string? accessToken { get; init; }
        public string redditUrl { get; init; }
    }
}
