namespace Dataset.Options;

internal record RepositoryOptionsBase
{
    public required string Owner { get; set; }
    public required string Repo { get; set; }
}
