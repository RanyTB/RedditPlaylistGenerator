﻿using System.Text.Json.Serialization;

namespace RedditPlaylistGenerator.Model
{
    public record RedditAccessToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }

        [JsonPropertyName("token_type")]

        public string TokenType { get; init; }
        [JsonPropertyName("expires_in")]

        public int ExpiresIn { get; init; }
        [JsonPropertyName("scope")]

        public string Scope { get; init; }


    }
}
