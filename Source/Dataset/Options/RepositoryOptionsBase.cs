namespace Dataset.Options;

internal record RepositoryOptionsBase
{
    public required string Owner { get; init; }
    public required string Repo { get; init; }
    public List<string>? Labels { get; init; }
}
