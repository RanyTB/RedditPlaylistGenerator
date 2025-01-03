﻿using Microsoft.AspNetCore.Mvc;
using RedditPlaylistGenerator.Model;
using RedditPlaylistGenerator.Services;

namespace RedditPlaylistGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedditController : ControllerBase
    {

        private readonly ILogger<RedditController> _logger;
        private readonly RedditService _redditService;
        private readonly SpotifyService _spotifyService;

        public RedditController(ILogger<RedditController> logger, RedditService redditService, SpotifyService spotifyService)
        {
            _logger = logger;
            _redditService = redditService;
            _spotifyService = spotifyService;
        }

        [HttpPost]
        public async Task<string> GeneratePlaylistFromRedditUrl([FromBody] GeneratePlaylistRequestBody body)
        {
            var songNamesResult = await _redditService.GetSongNames(body.redditUrl);

            var playlistLink = await _spotifyService.GeneratePlaylist(body.accessToken, songNamesResult);

            return playlistLink;

        }
    }
}
