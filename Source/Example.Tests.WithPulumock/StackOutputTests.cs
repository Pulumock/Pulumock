using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumock.Extensions;
using Pulumock.Mocks.Builders;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class StackOutputTests : IStackOutputTests
{
    [Theory]
    [InlineData("https://mocked.vault.azure.net/")]
    [InlineData("https://other.vault.azure.net/")]
    public async Task ShouldBeTestable_StackOutputValue(string mockedVaultUri)
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockResource(new MockResourceBuilder<Vault>()
                .WithOutput<VaultPropertiesResponse, string>(
                    x => x.Properties, 
                    x => x.VaultUri,
                    mockedVaultUri)
                .Build())
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());

        Vault keyVault = fixture.StackResources.Require<Vault>("microservice-kvws-kv");
        VaultPropertiesResponse keyVaultProperties = await keyVault.Properties.GetValueAsync();
        
        string keyVaultUriStackOutput = await fixture.StackOutputs.RequireValueAsync<string>("keyVaultUri");

        keyVaultUriStackOutput.ShouldBe(keyVaultProperties.VaultUri);
        keyVaultUriStackOutput.ShouldBe(mockedVaultUri);
    }
}
