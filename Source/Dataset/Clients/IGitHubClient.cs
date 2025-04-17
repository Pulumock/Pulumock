using Dataset.Clients.Responses;
using Dataset.Options;
using Microsoft.Extensions.Options;

namespace Dataset.Clients;

internal interface IGitHubClient
{
    public Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync<T>(IOptions<T> options, int page, int pageSize) where T : RepositoryOptionsBase;
}
