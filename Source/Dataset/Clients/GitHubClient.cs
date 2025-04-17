using System.Net.Http.Json;
using Dataset.Clients.Responses;
using Dataset.Options;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Dataset.Clients;

internal sealed class GitHubClient(HttpClient httpClient) : IGitHubClient
{
    public async Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync<T>(IOptions<T> options) where T : RepositoryOptionsBase
    {
        HttpResponseMessage result = await httpClient.GetAsync(BuildGetGitHubIssuesAsyncRelativeUrl(options));
        if (!result.IsSuccessStatusCode)
        {
            string errorResponse = await result.Content.ReadAsStringAsync();

            throw new InvalidOperationException(
                $"Failed to get issues from GitHub: {result.ReasonPhrase}\nResponse body:\n{errorResponse}");
        }
        
        List<GetGitHubIssuesAsyncResponse>? successResponse = await result.Content.ReadFromJsonAsync<List<GetGitHubIssuesAsyncResponse>>();
        if (successResponse is null)
        {
            throw new InvalidOperationException(
                "Failed to deserialize issues from GitHub.");
        }
        
        return successResponse.AsReadOnly();
    }

    private static Uri BuildGetGitHubIssuesAsyncRelativeUrl<T>(IOptions<T> options) where T : RepositoryOptionsBase
    {
        var query = new Dictionary<string, string?>();
        if (options.Value.Labels is not null && options.Value.Labels.Count > 0)
        {
            query.Add("labels", string.Join(",", options.Value.Labels));
        }
        
        string relativeUrl = QueryHelpers.AddQueryString($"repos/{options.Value.Owner}/{options.Value.Repo}/issues", query);

        return new Uri(relativeUrl, UriKind.Relative);
    }
}
