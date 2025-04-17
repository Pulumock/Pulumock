using Dataset.Options;

namespace Dataset.Services;

internal interface IGitHubMiner
{
    public Task<GitHubMinerResult> GetGitHubIssuesAsync();
}

internal interface IGitHubMiner<T>: IGitHubMiner where T : RepositoryOptionsBase;
