using MainExample.Stacks;
using MainExample.Tests.Shared.Interfaces;
using MainExample.Tests.WithPulumock.Shared;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures;

namespace MainExample.Tests.WithPulumock;

public class ConfigurationTests : TestBase, IConfigurationTests
{
    [Fact]
    public async Task Config_MockedConfigurationInResource()
    {
        Fixture fixture = await FixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(StackName));
        
        // ResourceSnapshot resourceSnapshot = fixture.ResourceSnapshots.Single(x => x.LogicalName.Equals("microservice-kv-vault", StringComparison.Ordinal));
        
        // ResourceSnapshot resourceSnapshot = mocks.ResourceSnapshots.Single(x => x.LogicalName.Equals("microservice-kv-vault", StringComparison.Ordinal));
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
    
    [Fact]
    public Task Config_MockedSecretInResource() => throw new NotImplementedException();
}
