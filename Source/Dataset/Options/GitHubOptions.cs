namespace Dataset.Options;

internal sealed record GitHubOptions
{
    public const string Key = "GitHub";
        
    public required string Token { get; set; }
}
