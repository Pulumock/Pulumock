using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumock.Extensions;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures;

namespace Example.Tests.WithPulumock;

public class ConfigurationTests : TestBase, IConfigurationTests
{
    [Fact]
    public async Task Config_MockedConfigurationInResource()
    {
        Fixture fixture = await FixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(StackName));
        
        ResourceSnapshot resourceSnapshot = fixture.ResourceSnapshots.Require("microservice-kvws-vault");
        resourceSnapshot.Inputs.TryGetValue("properties", out object? propertiesObj);
        
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
    
    // Modify config
    // Without mocked config -> throws
    
    [Fact]
    public Task Config_MockedSecretInResource() => throw new NotImplementedException();
}
