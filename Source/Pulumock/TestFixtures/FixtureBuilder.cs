using System.Collections.Immutable;
using System.Text.Json;
using Pulumi;
using Pulumi.Testing;
using Pulumock.Mocks.Enums;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures.Constants;

namespace Pulumock.TestFixtures;

// TODO: full/partial upsert and delete
// TODO: support both typed and non-typed builders and with() methods
public class FixtureBuilder
{
    private readonly Dictionary<string, object> _mockStackConfigurations = new();
    private readonly Dictionary<(Type Type, string? LogicalName), MockResource> _mockResources = new();
    private readonly Dictionary<MockCallToken, MockCall> _mockCalls = new();
    
    public FixtureBuilder WithMockStackConfiguration(string key, object value)
    {
        _mockStackConfigurations[key] = value;
        return this;
    }
    
    /// <summary>
    /// Adds a configuration key and value under a specific namespace.
    /// Also supports <see href="https://www.pulumi.com/docs/iac/concepts/config/#structured-configuration">structured configuration</see>.
    /// </summary>
    /// <param name="namespace">The configuration namespace (e.g., "project", "azure-native").</param>
    /// <param name="value">The configuration value.</param>
    /// <param name="keyName">The optional key name within the namespace.</param>
    public FixtureBuilder WithMockStackConfiguration(PulumiConfigurationNamespace @namespace, string keyName, object value)
    {
        WithMockStackConfiguration(FormatKey(@namespace.Value, keyName), value);
        return this;
    }
    
    public FixtureBuilder WithoutMockStackConfiguration(string key)
    {
        _mockStackConfigurations.Remove(key);
        return this;
    }
    
    public FixtureBuilder WithoutMockStackConfiguration(PulumiConfigurationNamespace @namespace, string keyName)
    {
        WithoutMockStackConfiguration(FormatKey(@namespace.Value, keyName));
        return this;
    }
    
    public FixtureBuilder ClearMockStackConfigurations()
    {
        _mockStackConfigurations.Clear();
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
        if (_mockStackConfigurations.Count > 0)
        {
            Environment.SetEnvironmentVariable(PulumiConfigurationConstants.EnvironmentVariable, 
                JsonSerializer.Serialize(_mockStackConfigurations));
        }
        
        var mocks = new Mocks.Mocks(_mockResources.ToImmutableDictionary(), _mockCalls.ToImmutableDictionary());
        
        (ImmutableArray<Resource> stackResources, IDictionary<string, object?> stackOutputs) = await Deployment.TestAsync(
            mocks,
            testOptions ?? new TestOptions { IsPreview = false, StackName = "dev" },
            async () => await createResourcesFunc());

        return new Fixture(stackResources, stackOutputs.ToImmutableDictionary(), mocks.ResourceSnapshots, mocks.CallSnapshots);
    }
    
    private static string FormatKey(string @namespace, string? keyName) =>
        string.IsNullOrWhiteSpace(keyName) ? @namespace : $"{@namespace}:{keyName}";
}
