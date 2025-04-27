using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumock.Extensions;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class StackOutputTests : IStackOutputTests
{
    [Fact]
    public async Task ShouldBeTestable_StackOutputValue()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());

        // TODO: simplify this with an extension to get nested property
        Vault keyVault = fixture.StackResources.Require<Vault>("microservice-kvws-kv");
        VaultPropertiesResponse keyVaultProperties = await keyVault.Properties.GetValueAsync();
        
        // TODO: simplify this with an extension
        if (!fixture.StackOutputs.TryGetValue("keyVaultUri", out object? output)
            || output is not Output<string> keyVaultUriStackOutput)
        {
            throw new InvalidOperationException("keyVaultUri was not an Output<string>");
        }

        (await keyVaultUriStackOutput.GetValueAsync()).ShouldBe(keyVaultProperties.VaultUri);
    }
}
