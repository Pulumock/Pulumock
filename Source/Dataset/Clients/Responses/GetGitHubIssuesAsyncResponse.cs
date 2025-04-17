namespace Dataset.Clients.Responses;

internal sealed record GetGitHubIssuesAsyncResponse
{
    public required string Title { get; init; }
}
