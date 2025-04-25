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
    private readonly Dictionary<(Type Type, string? LogicalName), MockResource> _mockResources = new();
    private readonly Dictionary<MockCallToken, MockCall> _mockCalls = new();

    // TODO: enable adding/removing single values
    public FixtureBuilder WithMockConfiguration(MockConfiguration mockConfiguration)
    {
        _mockConfiguration = mockConfiguration;
        return this;
    }
    
    public FixtureBuilder WithoutMockConfiguration()
    {
        _mockConfiguration = null;
        return this;
    }
    
    public FixtureBuilder WithMockStackReference(MockStackReference mockStackReference)
    {
        _mockResources[(mockStackReference.Type, mockStackReference.FullyQualifiedStackName)] = mockStackReference;
        return this;
    }
    
    public FixtureBuilder WithoutMockStackReference(MockStackReference mockStackReference)
    {
        _mockResources.Remove((mockStackReference.Type, mockStackReference.FullyQualifiedStackName));
        return this;
    }
    
    public FixtureBuilder WithMockResource(MockResource mockResource)
    {
        _mockResources[(mockResource.Type, mockResource.LogicalName)] = mockResource;
        return this;
    }
    
    public FixtureBuilder WithoutMockResource(MockResource mockResource)
    {
        _mockResources.Remove((mockResource.Type, mockResource.LogicalName));
        return this;
    }
    
    public FixtureBuilder WithMockCall(MockCall mockCall)
    {
        MockCallToken newKey = mockCall.Token;

        foreach (MockCallToken existingKey in _mockCalls.Keys
                     .Where(existingKey => existingKey.ConflictsWith(newKey)))
        {
            _mockCalls.Remove(existingKey);
            break;
        }

        _mockCalls[newKey] = mockCall;
        return this;
    }
    
    public FixtureBuilder WithoutMockCall(MockCall mockCall)
    {
        MockCallToken? existingKey = _mockCalls.Keys
            .FirstOrDefault(k => k.ConflictsWith(mockCall.Token));

        if (existingKey is not null)
        {
            _mockCalls.Remove(existingKey);
        }

        return this;
    }
        
    public async Task<Fixture> BuildAsync(Func<Task<IDictionary<string, object?>>> createResourcesFunc, TestOptions? testOptions = null)
    {
        if (_mockConfiguration is not null)
        {
            Environment.SetEnvironmentVariable(PulumiConfigurationConstants.EnvironmentVariable, 
                JsonSerializer.Serialize(_mockConfiguration.MockConfigurations));
        }
        
        var mocks = new Mocks.Mocks(_mockResources.ToImmutableDictionary(), _mockCalls.ToImmutableDictionary());
        
        (ImmutableArray<Resource> stackResources, IDictionary<string, object?> stackOutputs) = await Deployment.TestAsync(
            mocks,
            testOptions ?? new TestOptions { IsPreview = false },
            async () => await createResourcesFunc());

        return new Fixture(stackResources, stackOutputs.ToImmutableDictionary(), mocks.ResourceSnapshots, mocks.CallSnapshots);
    }
}
