using Dataset.Clients;
using Dataset.Clients.Responses;
using Dataset.Options;
using Microsoft.Extensions.Options;

namespace Dataset.Services;

internal sealed class GitHubIssueMiner<T>(IGitHubClient gitHubClient, IOptions<T> options) : IGitHubIssueMiner<T> where T : RepositoryOptionsBase
{
    // TODO: add to CSV file
    public async Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync() => await gitHubClient.GetGitHubIssuesAsync(options);
}
