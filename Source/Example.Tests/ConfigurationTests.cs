using System.Collections.Immutable;
using Example.Stacks;
using Example.Tests.Shared;
using Example.Tests.Shared.Interfaces;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace Example.Tests;

public sealed class ConfigurationTests : TestBase, IConfigurationTests
{
    [Fact]
    public async Task Config_MockedConfigurationInResource()
    {
        var mocks = new Mocks.Mocks();
        _ = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
        ResourceSnapshot resourceSnapshot = mocks.ResourceSnapshots.Single(x => x.LogicalName.Equals("microservice-kv-vault", StringComparison.Ordinal));
        if (!resourceSnapshot.Inputs.TryGetValue("properties", out object? propertiesObj) ||
            propertiesObj is not IDictionary<string, object> properties)
        {
            throw new KeyNotFoundException("Input with key 'properties' was not found or is not of type string.");
        }
        
        if (!properties.TryGetValue("tenantId", out object? tenantIdObj) ||
            tenantIdObj is not string tenantId)
        {
            throw new KeyNotFoundException("Input with key 'tenantId' was not found or is not of type string.");
        }
        
        tenantId.ShouldBe("1f526cdb-1975-4248-ab0f-57813df294cb");
    }
    
    [Fact]
    public async Task Config_MockedSecretInResource()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks.Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        Secret secret = result.Resources
            .OfType<Secret>()
            .Single(x => x.GetResourceName().Equals("microservice-kv-secret-Database--ConnectionString", StringComparison.Ordinal));
        
        SecretPropertiesResponse secretProperties = await OutputUtilities.GetValueAsync(secret.Properties);
        secretProperties.Value.ShouldBe("very-secret-value");
    }
}
