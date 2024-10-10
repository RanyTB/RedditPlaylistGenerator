using Microsoft.Extensions.Options;
using RedditPlaylistGenerator.Model;
using RedditPlaylistGenerator.Options;

namespace RedditPlaylistGenerator.Services
{
    public class SpotifyService(HttpClient _client, IOptions<SpotifyOptions> _spotifyOptions)
    {

        public async Task<string> GeneratePlaylist(string code, string codeVerifier, IList<string> songNames)
        {

            var token = await GetAccessToken(code, codeVerifier);

            return "hello world";

        }

        private async Task<string> GetAccessToken(string code, string codeVerifier)
        {

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _spotifyOptions.Value.ClientId,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = "http://localhost:5173",
                ["code_verifier"] = codeVerifier
            });

            request.Content = content;
            request.Content.Headers.Remove("Content-Type");
            request.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var res = await _client.SendAsync(request);

            res.EnsureSuccessStatusCode();

            var token = await res.Content.ReadFromJsonAsync<SpotifyAccessToken>();

            if (token == null)
            {
                throw new HttpRequestException("Failed to retrieve spotify access token.");
            }

            return token.AccessToken;
        }


    }
}
