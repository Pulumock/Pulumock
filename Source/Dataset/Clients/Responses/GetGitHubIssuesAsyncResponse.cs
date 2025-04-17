using System.Text.Json.Serialization;

namespace Dataset.Clients.Responses;

internal sealed record GetGitHubIssuesAsyncResponse
{
    public required string State { get; init; }
   
    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; init; }
    public required string Title { get; init; }
    public required string Body { get; init; }
    
    
    [JsonPropertyName("pull_request")]
    public object? PullRequest { get; set; }
}
