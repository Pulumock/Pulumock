using Dataset.Options;
using Microsoft.Extensions.Options;

namespace Dataset.Services;

internal sealed class GitHubIssueMiner(IOptions<GitHubOptions> options) : IGitHubIssueMiner
{
    public string GetResponse() => options.Value.Token;
}
