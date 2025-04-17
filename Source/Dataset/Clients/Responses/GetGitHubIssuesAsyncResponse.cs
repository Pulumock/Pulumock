using System.Text.Json.Serialization;

namespace Dataset.Clients.Responses;

internal sealed record GetGitHubIssuesAsyncResponse
{
    public required string Title { get; init; }
    
    [JsonPropertyName("pull_request")]
    public object? PullRequest { get; set; }
}
