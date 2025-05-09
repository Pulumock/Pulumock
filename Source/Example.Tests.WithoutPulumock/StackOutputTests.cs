using System.Collections.Immutable;
using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithoutPulumock.Mocks;
using Example.Tests.WithoutPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace Example.Tests.WithoutPulumock;

public class StackOutputTests : TestBase, IStackOutputTests
{
    [Theory]
    [InlineData("https://mocked.vault.azure.net/")]
    [InlineData("https://other.vault.azure.net/")]
    public async Task ShouldBeTestable_StackOutputValue(string mockedVaultUri)
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new MocksStackOutputTests(mockedVaultUri), 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());
        
        Vault keyVault = result.Resources
            .OfType<Vault>()
            .Single(x => x.GetResourceName().Equals("microservice-kvws-kv", StringComparison.Ordinal));
        
        VaultPropertiesResponse keyVaultProperties = await OutputUtilities.GetValueAsync(keyVault.Properties);
        
        string keyVaultUriStackOutput = result.StackOutputs["keyVaultUri"] is Output<string> vaultUriOutput
            ? await OutputUtilities.GetValueAsync(vaultUriOutput)
            : throw new InvalidOperationException("keyVaultUri was not an Output<string>");

        keyVaultUriStackOutput.ShouldBe(keyVaultProperties.VaultUri);
        keyVaultUriStackOutput.ShouldBe(mockedVaultUri);
    }
}
