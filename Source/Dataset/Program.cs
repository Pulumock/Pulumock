using Dataset.Options;
using Dataset.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddUserSecrets<Program>()
    .Build();

var services = new ServiceCollection();

services.Configure<GitHubOptions>(options => configuration.GetSection(GitHubOptions.Key).Bind(options));
services.AddTransient<IGitHubIssueMiner, GitHubIssueMiner>();

ServiceProvider provider = services.BuildServiceProvider();

IGitHubIssueMiner miner = provider.GetRequiredService<IGitHubIssueMiner>();
string res = miner.GetResponse();

Console.WriteLine(res);
