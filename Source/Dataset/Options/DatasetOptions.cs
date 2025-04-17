namespace Dataset.Options;

internal sealed class DatasetOptions
{
    public const string Key = "Dataset";
    
    public required string FilePath { get; init; }
}
