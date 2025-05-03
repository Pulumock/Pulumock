using System.Collections.Immutable;
using System.Text.Json;
using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithoutPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace Example.Tests.WithoutPulumock;

public sealed class ConfigurationTests : TestBase, IConfigurationTests
{
    [Fact]
    public async Task ShouldBeTestable_ConfigurationValue()
    {
        const string expectedTenantId = "1f526cdb-1975-4248-ab0f-57813df294cb";
        string? existingConfigJson = Environment.GetEnvironmentVariable("PULUMI_CONFIG");
        Dictionary<string, object> config = existingConfigJson != null
            ? JsonSerializer.Deserialize<Dictionary<string, object>>(existingConfigJson)!
            : new Dictionary<string, object>();
        
        config["azure-native:tenantId"] = expectedTenantId;
        
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(config));
        
        var mocks = new Mocks.Mocks();
        _ = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());
        
        EnrichedResource enrichedResource = mocks.EnrichedResources.Single(x => x.LogicalName.Equals("microservice-kvws-kv", StringComparison.Ordinal));
        if (!enrichedResource.Inputs.TryGetValue("properties", out object? propertiesObj) ||
            propertiesObj is not IDictionary<string, object> properties)
        {
            throw new KeyNotFoundException("Input with key 'properties' was not found or is not of type string.");
        }
        
        if (!properties.TryGetValue("tenantId", out object? tenantIdObj) ||
            tenantIdObj is not string tenantId)
        {
            throw new KeyNotFoundException("Input with key 'tenantId' was not found or is not of type string.");
        }
        
        tenantId.ShouldBe(expectedTenantId);
    }
    
    [Fact]
    public async Task ShouldBeTestable_SecretConfigurationValue()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks.Mocks(), 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());

        Secret secret = result.Resources
            .OfType<Secret>()
            .Single(x => x.GetResourceName().Equals("microservice-kvws-secret-Database--ConnectionString", StringComparison.Ordinal));
        
        SecretPropertiesResponse secretProperties = await OutputUtilities.GetValueAsync(secret.Properties);
        secretProperties.Value.ShouldBe("very-secret-value");
    }

    [Theory]
    [InlineData("75abe3bd-31dd-43be-bdfa-f4e937fac121")]
    [InlineData("e7c808e6-e111-4d82-b023-4075c2eee383")]
    [InlineData("4e5fdc58-8df1-43ab-b15f-ae7aeb7c45f7")]
    public async Task ShouldBeTestable_DynamicOverriddenConfigurationValue(string tenantId)
    {
        string? existingConfigJson = Environment.GetEnvironmentVariable("PULUMI_CONFIG");
        Dictionary<string, object> config = existingConfigJson != null
            ? JsonSerializer.Deserialize<Dictionary<string, object>>(existingConfigJson)!
            : new Dictionary<string, object>();
        
        config["azure-native:tenantId"] = tenantId;
        
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(config));
        
        var mocks = new Mocks.Mocks();
        _ = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());
        
        EnrichedResource vaultEnrichedResource = mocks.EnrichedResources.Single(x => 
            x.LogicalName.Equals("microservice-kvws-kv", StringComparison.Ordinal));
        
        if (!vaultEnrichedResource.Inputs.TryGetValue("properties", out object? propertiesObj) ||
            propertiesObj is not IDictionary<string, object> vaultProperties)
        {
            throw new KeyNotFoundException("Input with key 'properties' was not found or is not of type string.");
        }
        
        if (!vaultProperties.TryGetValue("tenantId", out object? tenantIdObj) ||
            tenantIdObj is not string vaultTenantId)
        {
            throw new KeyNotFoundException("Input with key 'tenantId' was not found or is not of type string.");
        }
        
        vaultTenantId.ShouldBe(tenantId);
    }

    [Fact]
    public Task ShouldBeTestable_MissingSingleRequiredConfigurationValue() =>
        Should.ThrowAsync<RunException>(async () =>
        {
            string? existingConfigJson = Environment.GetEnvironmentVariable("PULUMI_CONFIG");
            Dictionary<string, object> config = existingConfigJson != null
                ? JsonSerializer.Deserialize<Dictionary<string, object>>(existingConfigJson)!
                : new Dictionary<string, object>();
        
            config.Remove("azure-native:tenantId");
        
            Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(config));
            
            _ = await Deployment.TestAsync(
                new Mocks.Mocks(), 
                new TestOptions {IsPreview = false, StackName = DevStackName},
                async () => await CoreStack.DefineResourcesAsync());
        });

    [Fact]
    public Task ShouldBeTestable_MissingAllRequiredConfigurationValue() =>
        Should.ThrowAsync<RunException>(async () =>
        {
            Environment.SetEnvironmentVariable("PULUMI_CONFIG", null);
            
            _ = await Deployment.TestAsync(
                new Mocks.Mocks(), 
                new TestOptions {IsPreview = false, StackName = DevStackName},
                async () => await CoreStack.DefineResourcesAsync());
        });
}
