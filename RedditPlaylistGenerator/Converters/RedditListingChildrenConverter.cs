using RedditPlaylistGenerator.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RedditPlaylistGenerator.Converters
{
    public class RedditListingChildrenConverter : JsonConverter<IList<RedditListingEntry>?>
    {
        public override IList<RedditListingEntry>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected start of array token");
            }

            var children = new List<RedditListingEntry>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    continue;
                    
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var entry = JsonSerializer.Deserialize<RedditListingEntry>(ref reader, options);
                    children.Add(entry);
                }


            }

            return children;

        }

        public override void Write(Utf8JsonWriter writer, IList<RedditListingEntry>? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartArray();
            foreach (var entry in value)
            {
                JsonSerializer.Serialize(writer, entry, options);
            }
            writer.WriteEndArray();
        }
    }
}