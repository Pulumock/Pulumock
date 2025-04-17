using Dataset.Clients.Responses;

namespace Dataset.Services;

#pragma warning disable CA1303
internal sealed class DatasetGenerator(IEnumerable<IGitHubMiner> miners) : IDatasetGenerator
{
    public async Task GenerateDatasetAsync()
    {
        Console.WriteLine("Generating dataset...");
        
        Console.WriteLine($"Running {miners.Count()} miners in parallel...");
        GitHubMinerResult[] results = await Task.WhenAll(miners.Select(m => m.GetGitHubIssuesAsync()));

        foreach (GitHubMinerResult issueSet in results)
        {
            Console.WriteLine($"{issueSet.Issues.Count} issues found.");
        }
    }
}
#pragma warning restore CA1303
