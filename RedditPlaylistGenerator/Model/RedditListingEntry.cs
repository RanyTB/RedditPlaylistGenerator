using RedditPlaylistGenerator.Converters;
using System.Text.Json.Serialization;

namespace RedditPlaylistGenerator.Model
{

    public record RedditListingEntry
    {
        public string Kind { get; init; }
        public RedditListingData Data { get; init; }
    }

    public record RedditListingData
    {
        public string? Title { get; init; }

        [JsonConverter(typeof(RedditListingChildrenConverter))]
        public IList<RedditListingEntry>? Children { get; init; }

        //public RedditListingEntry? Replies { get; init; } This need to handle empty string case
        public string? Body { get; init; }



    }

}


