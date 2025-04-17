using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text.Json;
using Dataset.Clients.Responses;
using Dataset.Options;
using Microsoft.Extensions.Options;

namespace Dataset.Clients;

internal sealed class GitHubClient(HttpClient httpClient) : IGitHubClient
{
    public async Task<IReadOnlyCollection<GetGitHubIssuesAsyncResponse>> GetGitHubIssuesAsync<T>(IOptions<T> options) where T : RepositoryOptionsBase
    {
        HttpResponseMessage result = await httpClient.GetAsync(new Uri($"repos/{options.Value.Owner}/{options.Value.Repo}/issues", UriKind.Relative));
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
}
