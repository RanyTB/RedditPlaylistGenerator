using System.Text.Json.Serialization;

namespace RedditPlaylistGenerator.Model
{
  public record SpotifyPlaylist
  {
    public string Id { get; init; }

    [JsonPropertyName("external_urls")]
    public ExternalUrls ExternalUrls { get; init; }
  }

  public record ExternalUrls
  {
    public string Spotify { get; init; }
  }
}