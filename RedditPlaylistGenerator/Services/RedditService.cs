using RedditPlaylistGenerator.Model;
using System.Text.RegularExpressions;

namespace RedditPlaylistGenerator.Services
{
    public class RedditService(HttpClient _client)
    {

        public async Task<SongNamesResult> GetSongNames(string articleUrl)
        {
            var match = Regex.Match(articleUrl, @"reddit.com/r/[^/]+/comments/(?<id>[^/]+)");

            if (!match.Success)
            {
                throw new ArgumentException("Invalid reddit url.");
            }

            var postId = match.Groups["id"].Value;

            var commentTree = await GetCommentTree(postId);
            var comments = GetComments(commentTree);

            var postTitle = GetPostTitle(commentTree);
            var songNames = ExtractSongNames(comments);

            return new SongNamesResult()
            {
                PostTitle = postTitle,
                SongNames = songNames
            };
        }


        private async Task<IList<RedditListingEntry>> GetCommentTree(string postId)
        {
            var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"comments/{postId}?raw_json=1"));

            res.EnsureSuccessStatusCode();

            var listings = await res.Content.ReadFromJsonAsync<IList<RedditListingEntry>>();

            if (listings == null)
            {
                throw new HttpRequestException("Failed to retrieve comment tree.");
            }

            return listings;

        }

        private string? GetPostTitle(IList<RedditListingEntry> listings)
        {
            if (listings == null || listings.Count < 1)
            {
                throw new ArgumentException("Invalid listings data.");
            }

            return listings?[0]?.Data?.Children?[0]?.Data?.Title;
        }

        public static IList<string> GetComments(IList<RedditListingEntry> listings)
        {
            if (listings == null || listings.Count < 2)
            {
                throw new ArgumentException("Invalid listings data.");
            }

            var comments = listings[1].Data.Children;

            if (comments == null)
            {
                throw new ArgumentException("No comments found.");
            }


            var res = comments
                .Select(l => l.Data.Body)
                .OfType<string>()
                .ToList();

            return res;
        }


        public static IList<string> ExtractSongNames(IList<string> comments)
        {
            var songNames = new List<string>();

            foreach (var comment in comments)
            {
                var matches = Regex.Matches(comment, @"(.+)( +by +|- *|, *)(.+)");

                foreach (Match match in matches)
                {
                    var formattedSongName = $"{match.Groups[1].Value.Trim()} - {match.Groups[3].Value}";

                    songNames.Add(formattedSongName);
                }
            }

            return songNames;
        }


    }
}
