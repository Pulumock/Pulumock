using System.Collections.Immutable;
using MainExample.Stacks;
using MainExample.Tests.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace MainExample.Tests;

public class StackOutputTests : TestBase
{
    [Fact]
    public async Task StackOutputs_ShouldOutputMockedValue()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks.Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
        Vault keyVault = result.Resources
            .OfType<Vault>()
            .Single(x => x.GetResourceName().Equals("microservice-kv-vault", StringComparison.Ordinal));
        
        VaultPropertiesResponse keyVaultProperties = await OutputUtilities.GetValueAsync(keyVault.Properties);
        
        string keyVaultUriStackOutput = result.StackOutputs["keyVaultUri"] is Output<string> vaultUriOutput
            ? await OutputUtilities.GetValueAsync(vaultUriOutput)
            : throw new InvalidOperationException("keyVaultUri was not an Output<string>");

        keyVaultUriStackOutput.ShouldBe(keyVaultProperties.VaultUri);
    }
}
