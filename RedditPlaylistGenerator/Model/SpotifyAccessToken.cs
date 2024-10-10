using System.Text.Json.Serialization;

namespace RedditPlaylistGenerator.Model
{
    public record SpotifyAccessToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
        [JsonPropertyName("token_type")]
        string TokenType { get; init; }
        string Scope { get; init; }
        [JsonPropertyName("expires_in")]
        int ExpiresIn { get; init; }
        [JsonPropertyName("refresh_token")]
        string RefreshToken { get; init; }

    }
}
