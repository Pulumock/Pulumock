using System.Text.Json.Serialization;

namespace Dataset.Clients.Responses;

internal sealed record GetGitHubIssuesAsyncResponse
{
    [JsonPropertyName("html_url")]
    public required string HtmlUrl { get; init; } 
    public required string Title { get; init; }
    [JsonPropertyName("created_at")]
    public required DateTime CreatedAtUtc { get; init; }
    public required Reactions Reactions { get; init; }
    public required string Body { get; init; }
    
    /// <summary>
    /// GitHub's REST API considers every pull request an issue, but not every issue is a pull request.
    /// For this reason, "Issues" endpoints may return both issues and pull requests in the response.
    /// You can identify pull requests by the pull_request key (but it is never used in the code, thus just casting it to an object).
    /// </summary>
    [JsonPropertyName("pull_request")]
    public object? PullRequest { get; set; }
}

internal sealed record Reactions
{
    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }
    
    [JsonPropertyName("+1")]
    public required int ThumbsUp { get; init; }
}
