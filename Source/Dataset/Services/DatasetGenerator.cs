using Dataset.Clients.Responses;

namespace Dataset.Services;

#pragma warning disable CA1303
internal sealed class DatasetGenerator(IEnumerable<IGitHubMiner> miners) : IDatasetGenerator
{
    public async Task GenerateDatasetAsync()
    {
        Console.WriteLine("Generating dataset...");
        
        Console.WriteLine($"Running {miners.Count()} miners in parallel...");
        IReadOnlyCollection<GetGitHubIssuesAsyncResponse>[] results = await Task.WhenAll(miners.Select(m => m.GetGitHubIssuesAsync()));

        foreach (IReadOnlyCollection<GetGitHubIssuesAsyncResponse> issueSet in results)
        {
            Console.WriteLine(issueSet.FirstOrDefault()?.Title ?? "No issues");
        }
    }
}
#pragma warning restore CA1303
