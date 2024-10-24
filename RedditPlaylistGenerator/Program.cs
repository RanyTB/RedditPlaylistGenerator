using RedditPlaylistGenerator.Configuration;
using RedditPlaylistGenerator.DelegatingHandlers;
using RedditPlaylistGenerator.Options;
using RedditPlaylistGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RedditService>();

builder.Services.AddSingleton<SpotifyService>();

builder.Services.AddTransient<RedditAuthenticationHandler>();

builder.Services.Configure<RedditOptions>(builder.Configuration.GetSection(RedditOptions.Key));

builder.Services.Configure<SpotifyOptions>(builder.Configuration.GetSection(SpotifyOptions.Key));

builder.Services.AddHttpClient<RedditService>(client =>
{
    client.BaseAddress = new Uri("https://oauth.reddit.com");
    client.DefaultRequestHeaders.UserAgent.ParseAdd(builder.Configuration.GetSection(RedditOptions.Key)?.Get<RedditOptions>()?.UserAgent);

}).AddHttpMessageHandler<RedditAuthenticationHandler>();

builder.Services.AddHttpClient<SpotifyService>("SpotifyClient", client =>
{
    client.BaseAddress = new Uri("https://api.spotify.com/v1/");

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
