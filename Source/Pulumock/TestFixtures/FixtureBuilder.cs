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
    private readonly List<MockResource> _mockResources = [];
    private readonly List<MockCall> _mockCalls = [];

    public FixtureBuilder WithMockConfiguration(MockConfiguration mockConfiguration)
    {
        _mockConfiguration = mockConfiguration;
        return this;
    }
    
    public FixtureBuilder WithMockStackReference(MockStackReference mockStackReference)
    {
        _mockResources.Add(mockStackReference);
        return this;
    }
    
    public FixtureBuilder WithMockResource(MockResource mockResource)
    {
        _mockResources.Add(mockResource);
        return this;
    }
    
    public FixtureBuilder WithMockCall(MockCall mockCall)
    {
        _mockCalls.Add(mockCall);
        return this;
    }
        
    public async Task<Fixture> BuildAsync(Func<Task<IDictionary<string, object?>>> createResourcesFunc, TestOptions? testOptions = null)
    {
        if (_mockConfiguration is not null)
        {
            Environment.SetEnvironmentVariable(PulumiConfigurationConstants.EnvironmentVariable, 
                JsonSerializer.Serialize(_mockConfiguration.MockConfigurations));
        }
        
        var mocks = new Mocks.Mocks(_mockResources, _mockCalls);
        
        (ImmutableArray<Resource> stackResources, IDictionary<string, object?> stackOutputs) = await Deployment.TestAsync(
            mocks,
            testOptions ?? new TestOptions { IsPreview = false },
            async () => await createResourcesFunc());

        return new Fixture(stackResources, stackOutputs.ToImmutableDictionary(), mocks.ResourceSnapshots, mocks.CallSnapshots);
    }
}
