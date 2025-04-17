using Dataset.Clients;
using Dataset.Clients.Responses;
using Dataset.Options;
using Microsoft.Extensions.Options;

namespace Dataset.Services;

internal sealed class GitHubMiner<T>(IGitHubClient gitHubClient, IOptions<T> options) : IGitHubMiner<T> where T : RepositoryOptionsBase
{
    public async Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync() => await gitHubClient.GetGitHubIssuesAsync(options);
}
