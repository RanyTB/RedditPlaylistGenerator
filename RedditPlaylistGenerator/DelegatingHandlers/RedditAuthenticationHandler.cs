
using Microsoft.Extensions.Options;
using RedditPlaylistGenerator.Configuration;
using RedditPlaylistGenerator.Model;
using System.Net.Http.Headers;
using System.Text;

namespace RedditPlaylistGenerator.DelegatingHandlers
{
    public class RedditAuthenticationHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly RedditOptions _redditOptions;

        public RedditAuthenticationHandler(HttpClient httpClient, IOptions<RedditOptions> redditOptions)
        {
            _httpClient = httpClient;
            _redditOptions = redditOptions.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await GetAccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetAccessToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.reddit.com/api/v1/access_token?grant_type=client_credentials");
            var Base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_redditOptions.ClientId}:{_redditOptions.ClientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Base64Credentials);
            request.Headers.UserAgent.ParseAdd(_redditOptions.UserAgent);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var accessTokenResponse = await response.Content.ReadFromJsonAsync<RedditAccessToken>();

            if (accessTokenResponse == null)
            {
                throw new HttpRequestException("Failed to retrieve access token.");
            }

            return accessTokenResponse.AccessToken;
        }


    }
}
