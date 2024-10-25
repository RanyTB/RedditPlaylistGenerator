namespace RedditPlaylistGenerator.Model
{
    public record SpotifySearch
    {
        public SpotifyTracks Tracks { get; init; }
    }

    public record SpotifyTracks
    {
        public IList<Item> Items { get; init; }
    }

    public record Item
    {
        public string Uri { get; init; }
    }
}