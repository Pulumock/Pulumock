using System.Collections.Immutable;
using System.Text.Json;
using Pulumi;
using Pulumi.Testing;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures.Constants;

namespace Pulumock.TestFixtures;

public class FixtureBuilder
{
    private MockConfiguration? _mockConfiguration;

    public FixtureBuilder WithMockConfiguration(MockConfiguration mockConfiguration)
    {
        _mockConfiguration = mockConfiguration;
        return this;
    }
        
    public async Task<Fixture> BuildAsync(Func<Task<IDictionary<string, object?>>> createResourcesFunc, TestOptions? testOptions = null)
    {
        if (_mockConfiguration is not null)
        {
            Environment.SetEnvironmentVariable(PulumiConfigurationConstants.EnvironmentVariable, 
                JsonSerializer.Serialize(_mockConfiguration.MockConfigurations));
        }
        
        (ImmutableArray<Resource> stackResources, IDictionary<string, object?> stackOutputs) = await Deployment.TestAsync(
            new EmptyMocks(), 
            testOptions ?? new TestOptions {IsPreview = false},
            async () => await createResourcesFunc());
        
        return new Fixture(stackResources, stackOutputs.ToImmutableDictionary());
    }
}
