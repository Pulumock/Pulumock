using System.Collections.Immutable;
using System.Text.Json;
using Pulumi;
using Pulumi.Testing;
using Pulumock.Mocks.Builders;
using Pulumock.Mocks.Enums;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures.Constants;
using Pulumock.Utilities;

namespace Pulumock.TestFixtures;

/// <summary>
/// A fluent builder for creating Pulumock test <see cref="Fixture"/> instances.
/// Allows injection of dynamic mocked stack configuration, references, resources, and calls to simulate Pulumi stack behavior.
/// </summary>
public class FixtureBuilder
{
    private readonly Dictionary<string, object> _mockStackConfigurations = new();
    private readonly Dictionary<(Type Type, string? LogicalName), MockResource> _mockResources = new();
    private readonly Dictionary<MockCallToken, MockCall> _mockCalls = new();
    
    /// <summary>
    /// Adds a configuration key-value pair to the stack configuration.
    /// </summary>
    public FixtureBuilder WithMockStackConfiguration(string key, object value)
    {
        _mockStackConfigurations[key] = value;
        return this;
    }
    
    /// <summary>
    /// Adds a namespaced configuration value to the stack configuration.
    /// </summary>
    /// <param name="namespace">The config namespace (e.g., <see cref="PulumiConfigurationNamespace.AzureNative"/>).</param>
    /// <param name="keyName">The key name within the namespace.</param>
    /// <param name="value">The configuration value to mock.</param>
    public FixtureBuilder WithMockStackConfiguration(PulumiConfigurationNamespace @namespace, string keyName, object value)
    {
        WithMockStackConfiguration(ConfigurationKeyFormatter.Format(@namespace.Value, keyName), value);
        return this;
    }
    
    /// <summary>
    /// Removes a specific configuration key from the mocked stack configuration.
    /// </summary>
    public FixtureBuilder WithoutMockStackConfiguration(string key)
    {
        _mockStackConfigurations.Remove(key);
        return this;
    }
    
    /// <summary>
    /// Removes a namespaced configuration key from the mocked stack configuration.
    /// </summary>
    public FixtureBuilder WithoutMockStackConfiguration(PulumiConfigurationNamespace @namespace, string keyName)
    {
        WithoutMockStackConfiguration(ConfigurationKeyFormatter.Format(@namespace.Value, keyName));
        return this;
    }
    
    /// <summary>
    /// Clears all mocked stack configuration values.
    /// </summary>
    public FixtureBuilder ClearMockStackConfigurations()
    {
        _mockStackConfigurations.Clear();
        return this;
    }
    
    /// <summary>
    /// Adds a mocked <see cref="StackReference"/> to the fixture.
    /// </summary>
    public FixtureBuilder WithMockStackReference(MockStackReference mockStackReference)
    {
        _mockResources[(mockStackReference.Type, mockStackReference.FullyQualifiedStackName)] = mockStackReference;
        return this;
    }
    
    /// <summary>
    /// Removes a mocked <see cref="StackReference"/> by its fully qualified name.
    /// </summary>
    public FixtureBuilder WithoutMockStackReference(string fullyQualifiedStackName)
    {
        MockStackReference mock = new MockStackReferenceBuilder(fullyQualifiedStackName).Build();
        _mockResources.Remove((mock.Type, mock.FullyQualifiedStackName));
        return this;
    }
    
    /// <summary>
    /// Adds a mocked <see cref="Resource"/> to the fixture.
    /// </summary>
    public FixtureBuilder WithMockResource(MockResource mockResource)
    {
        _mockResources[(mockResource.Type, mockResource.LogicalName)] = mockResource;
        return this;
    }
    
    /// <summary>
    /// Removes a previously added mocked <see cref="Resource"/> from the fixture.
    /// </summary>
    public FixtureBuilder WithoutMockResource(MockResource mockResource)
    {
        _mockResources.Remove((mockResource.Type, mockResource.LogicalName));
        return this;
    }
    
    /// <summary>
    /// Adds a mocked function call to the fixture. Replaces any existing call with the same token.
    /// </summary>
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
    
    /// <summary>
    /// Removes a mocked function call from the fixture.
    /// </summary>
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
        
    /// <summary>
    /// Builds the fixture asynchronously using an async resource creation delegate.
    /// </summary>
    /// <param name="createResourcesFunc">The async function that defines the Pulumi resources.</param>
    /// <param name="testOptions">Optional test options, such as <c>StackName</c> or <c>IsPreview</c>.</param>
    /// <returns>A fully configured <see cref="Fixture"/> representing the test result.</returns>
    public async Task<Fixture> BuildAsync(Func<Task<IDictionary<string, object?>>> createResourcesFunc, TestOptions? testOptions = null)
    {
        Environment.SetEnvironmentVariable(PulumiConfigurationConstants.EnvironmentVariable,
            _mockStackConfigurations.Count > 0 ? JsonSerializer.Serialize(_mockStackConfigurations) : null);

        var mocks = new Mocks.Mocks(_mockResources.ToImmutableDictionary(), _mockCalls.ToImmutableDictionary());
        
        (ImmutableArray<Resource> resources, IDictionary<string, object?> registeredOutputs) = await Deployment.TestAsync(
            mocks,
            testOptions ?? new TestOptions { IsPreview = false, StackName = "dev" },
            async () => await createResourcesFunc());

        return new Fixture(resources, registeredOutputs.ToImmutableDictionary(), mocks.EnrichedResources, mocks.EnrichedCalls);
    }
    
    /// <summary>
    /// Builds the fixture using a synchronous resource creation delegate.
    /// </summary>
    /// <param name="createResourcesFunc">The sync function that defines the Pulumi resources.</param>
    /// <param name="testOptions">Optional test options, such as <c>StackName</c> or <c>IsPreview</c>.</param>
    /// <returns>A fully configured <see cref="Fixture"/> representing the test result.</returns>
    public async Task<Fixture> Build(Func<IDictionary<string, object?>> createResourcesFunc, TestOptions? testOptions = null)
    {
        Environment.SetEnvironmentVariable(PulumiConfigurationConstants.EnvironmentVariable,
            _mockStackConfigurations.Count > 0 ? JsonSerializer.Serialize(_mockStackConfigurations) : null);

        var mocks = new Mocks.Mocks(_mockResources.ToImmutableDictionary(), _mockCalls.ToImmutableDictionary());
        
        (ImmutableArray<Resource> resources, IDictionary<string, object?> registeredOutputs) = await Deployment.TestAsync(
            mocks,
            testOptions ?? new TestOptions { IsPreview = false, StackName = "dev" },
            createResourcesFunc);

        return new Fixture(resources, registeredOutputs.ToImmutableDictionary(), mocks.EnrichedResources, mocks.EnrichedCalls);
    }
}
