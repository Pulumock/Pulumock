# Dataset Generator

This tool extracts GitHub issues from selected repositories (currently [pulumi](https://github.com/pulumi/pulumi) and [proti](https://github.com/proti-iac/proti)) and compiles them into a dataset.

## Prerequisites

1. Create a [GitHub Fine-Grained Personal Access Token](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#fine-grained-personal-access-tokens):
   - Under "Repository access" select "Only select repositories" (public repositories are included by default)
   - Under "Repository permissions", set "Issues" to "Read-only"

2. Store the token using .NET user secrets:
   ```bash
   dotnet user-secrets set "GitHub:Token" "your_token_here"
   ```

## Running the Program

Run the application using the .NET CLI:
```bash
dotnet run
```

This will fetch issues from the configured repositories and generate a dataset.
You can configure the output directory in `appsettings.json`:
```markdown
"Dataset": {
    "FilePath": "your_file_path_here"
}
```

## GitHub API Reference

This project uses the GitHub Issues REST API:

https://docs.github.com/en/rest/issues/issues?apiVersion=2022-11-28#list-repository-issues
