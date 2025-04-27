using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumock.Extensions;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class ConfigurationTests : TestBase, IConfigurationTests
{
    [Fact]
    public async Task Config_MockedConfigurationInResource()
    {
        Fixture fixture = await FixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(StackName));
        
        ResourceSnapshot resourceSnapshot = fixture.ResourceSnapshots.Require("microservice-kvws-kv");
        
        string tenantId = resourceSnapshot.RequireInputValue<VaultArgs, VaultPropertiesArgs, string>(
            x => x.Properties, 
            y => y.TenantId);
        
        tenantId.ShouldBe("");

        // if (!resourceSnapshot.Inputs.TryGetValue("properties", out object? propertiesObj) ||
        //     propertiesObj is not IDictionary<string, object> properties)
        // {
        //     throw new KeyNotFoundException("Input with key 'properties' was not found or is not of type string.");
        // }
        //
        // if (!properties.TryGetValue("tenantId", out object? tenantIdObj) ||
        //     tenantIdObj is not string tenantId)
        // {
        //     throw new KeyNotFoundException("Input with key 'tenantId' was not found or is not of type string.");
        // }
        //
        // tenantId.ShouldBe("1f526cdb-1975-4248-ab0f-57813df294cb");
    }
    
    // Nested config
    
    // Modify config
    // Without mocked config -> throws
    
    [Fact]
    public Task Config_MockedSecretInResource() => throw new NotImplementedException();
}
