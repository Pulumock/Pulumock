using System.Net.Http.Headers;
using Dataset.Clients;
using Dataset.Options;
using Dataset.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddUserSecrets<Program>()
    .Build();

var services = new ServiceCollection();

services.Configure<GitHubOptions>(options => configuration.GetSection(GitHubOptions.Key).Bind(options));
services.Configure<PulumiRepositoryOptions>(options => configuration.GetSection(PulumiRepositoryOptions.Key).Bind(options));
services.AddHttpClient<IGitHubClient, GitHubClient>((provider, client) =>
{
    GitHubOptions gitHubOptions = provider.GetRequiredService<IOptions<GitHubOptions>>().Value;

    client.BaseAddress = new Uri("https://api.github.com");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gitHubOptions.Token);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Pulumock-Dataset", "1.0.0"));
});
services.AddTransient<IGitHubIssueMiner<PulumiRepositoryOptions>, GitHubIssueMiner<PulumiRepositoryOptions>>();

ServiceProvider provider = services.BuildServiceProvider();

// TODO: run from a DatasetGenerator class
IGitHubIssueMiner<PulumiRepositoryOptions> pulumiMiner = provider.GetRequiredService<IGitHubIssueMiner<PulumiRepositoryOptions>>();
