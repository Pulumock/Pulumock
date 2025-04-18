using System.Text.Json.Serialization;

namespace Dataset.Clients.Responses;

internal sealed record GetGitHubIssuesAsyncResponse
{
    [JsonPropertyName("html_url")]
    public required string HtmlUrl { get; init; } 
    public required string Title { get; init; }
    public required string Body { get; init; }
    
    
    [JsonPropertyName("pull_request")]
    public object? PullRequest { get; set; }
}
