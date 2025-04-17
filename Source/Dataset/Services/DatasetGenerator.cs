using Dataset.Options;
using Dataset.Utils;
using Microsoft.Extensions.Options;

namespace Dataset.Services;

#pragma warning disable CA1303
internal sealed class DatasetGenerator(IEnumerable<IGitHubMiner> miners, IOptions<DatasetOptions> options) : IDatasetGenerator
{
    public async Task GenerateDatasetAsync()
    {
        try
        {
            Console.WriteLine("Generating dataset...");
            
            Console.WriteLine($"Running {miners.Count()} miners in parallel...");
            GitHubMinerResult[] results = await Task.WhenAll(miners.Select(m => m.GetGitHubIssuesAsync()));

            Console.WriteLine("Exporting issues to workbook...");
            ExcelExporter.ExportToWorkbook(results, options.Value.FilePath);
            
            Console.WriteLine($"Completed successfully! The workbook can be found at path: '{options.Value.FilePath}'");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to generate dataset: {e.Message}");
            throw;
        }
    }
}
#pragma warning restore CA1303
