using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using RedditPlaylistGenerator.Model;
using RedditPlaylistGenerator.Options;
using System.Net.Http.Headers;

namespace RedditPlaylistGenerator.Services
{
    public class SpotifyService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly HttpClient _spotifyClient;
        private readonly IOptions<SpotifyOptions> _spotifyOptions;
        string? accessToken;

        public SpotifyService(HttpClient client, IHttpClientFactory clientFactory, IOptions<SpotifyOptions> spotifyOptions)
        {
            _client = client;
            _spotifyClient = clientFactory.CreateClient("SpotifyClient");
            _spotifyOptions = spotifyOptions;
        }

        public async Task<string> GeneratePlaylist(string code, string codeVerifier, IList<string> songNames, string playlistName)
        {
            accessToken = await GetAccessToken(code, codeVerifier);
            var songUris = await GetSongUris(songNames);

            //Search for every songName and extract the first song from the result (tracks.items[0].uri)
            //Create new playlist
            //Add the uri's to the playlist. The endpoint supports maximum 100 songs at a time, so split request if more than 100 results


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

        private async Task<IList<string>> GetSongUris(IList<string> songNames)
        {
            var songUriTasks = songNames
                .Select(GetSongUri);

            var results = await Task.WhenAll(songUriTasks);


            var songUris = results.ToList().Where(s => s != null).ToList();

            return songUris;
        }

        private async Task<string?> GetSongUri(string songName)
        {
            
            _spotifyClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var encodedSongName = Uri.EscapeDataString(songName);

            var res = await _spotifyClient.GetAsync($"search?q={encodedSongName}&type=track");

            if (res.StatusCode == System.Net.HttpStatusCode.TooManyRequests && res.Headers.TryGetValues("Retry-After", out var values))
            {
                var retryAfterStr = values.FirstOrDefault("30");
                var delaySeconds = int.Parse(retryAfterStr);
                await Task.Delay(delaySeconds * 1000);
                return await GetSongUri(songName);
            }

            if (!res.IsSuccessStatusCode)
            {
                throw new Exception("Failed to search for song.");
            }

            var responseContent = await res.Content.ReadFromJsonAsync<SpotifySearch>();

            if (responseContent == null)
            {
                throw new Exception("Failed to read response content.");
            }

            if (!responseContent.Tracks.Items.Any())
            {
                return null;
            }

            return responseContent.Tracks.Items[0].Uri;
        }


    }
}
