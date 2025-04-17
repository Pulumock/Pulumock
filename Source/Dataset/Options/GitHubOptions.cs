namespace Dataset.Options;

internal sealed record GitHubOptions
{
    public const string Key = "GitHub";
        
    public required Uri BaseUri { get; init; }
    public required string Token { get; init; }
    public required string MediaType { get; init; }
}
