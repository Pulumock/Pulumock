using Dataset.Clients.Responses;
using Dataset.Options;

namespace Dataset.Services;

internal interface IGitHubIssueMiner<T> where T : RepositoryOptionsBase
{
    public Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync();
}
