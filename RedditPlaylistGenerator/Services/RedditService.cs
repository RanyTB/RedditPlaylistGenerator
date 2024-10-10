using RedditPlaylistGenerator.Model;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace RedditPlaylistGenerator.Services
{
    public class RedditService(HttpClient _client)
    {

        public async Task<IList<string>> GetSongNames(string articleUrl)
        {
            var commentTree = await GetCommentTree("1g0c1pc");
            var comments = GetComments(commentTree);

            return ExtractSongNames(comments);
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
                var matches = Regex.Matches(comment, @"(.+)(\sby\s|\s*?-\s*?|,\s)(.+)");

                foreach (Match match in matches)
                {
                    songNames.Add(match.Value);
                }
            }

            return songNames;
        }


    }
}
