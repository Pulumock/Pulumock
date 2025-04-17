using Dataset.Clients.Responses;
using Dataset.Options;

namespace Dataset.Services;

internal interface IGitHubMiner
{
    public Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync();
}

internal interface IGitHubMiner<T>: IGitHubMiner where T : RepositoryOptionsBase;
