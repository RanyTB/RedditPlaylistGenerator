namespace RedditPlaylistGenerator.Model
{
  public record SongNamesResult
  {
    public string? PostTitle { get; init; }
    public IList<string> SongNames { get; init; }
  }
}