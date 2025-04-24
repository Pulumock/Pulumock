using System.Collections.Immutable;
using System.Text.Json;
using MainExample.Stacks;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;
using Deployment = Pulumi.Deployment;
using Resource = Pulumi.Resource;

namespace MainExample.Tests;

// TODO: test stack
// - Resource
//   - Input only
//   - Output only
//   - Input & Output
//   - Resource dependencies
// - Call
//   - With/Without args
//   - Resource dependencies on call
// - Creates Vault with/without component resource
    
// TODO: test component resource
// - Required args
// - Parent
public class ProgramTests
{
    private const string StackName = "dev";
    
    public ProgramTests() => 
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "azure-native:tenantId", "1f526cdb-1975-4248-ab0f-57813df294cb" },
            { "azure-native:subscriptionId", "f2f2c6e5-17c2-4dfa-913d-6509deb6becf" },
            { "azure-native:location", "swedencentral" },
            { "project:useKeyVaultWithSecretsComponentResource", "true" },
            { "project:databaseConnectionString", "very-secret-value" }
        }));
    
    [Fact]
    public async Task Config_ShouldUseMockedConfigurationInResource()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        Vault keyVault = result.Resources
            .OfType<Vault>()
            .Single(x => x.GetResourceName().Equals("microservice-kv-vault", StringComparison.Ordinal));
        
        VaultPropertiesResponse keyVaultProperties = await OutputUtilities.GetValueAsync(keyVault.Properties);
        keyVaultProperties.TenantId.ShouldBe("1f526cdb-1975-4248-ab0f-57813df294cb");
    }
    
    [Fact]
    public async Task Config_ShouldUseMockedConfigurationSecretInResource()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        Secret secret = result.Resources
            .OfType<Secret>()
            .Single(x => x.GetResourceName().Equals("microservice-kv-secret-Database--ConnectionString", StringComparison.Ordinal));
        
        SecretPropertiesResponse secretProperties = await OutputUtilities.GetValueAsync(secret.Properties);
        secretProperties.Value.ShouldBe("very-secret-value");
    }
    
    [Fact]
    public async Task StackReference_ShouldUseMockedStackReferenceInResource()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        RoleAssignment roleAssignment = result.Resources
            .OfType<RoleAssignment>()
            .Single(x => x.GetResourceName().Equals("microservice-ra-kvReader", StringComparison.Ordinal));
        
        string principalId = await OutputUtilities.GetValueAsync(roleAssignment.PrincipalId);
        principalId.ShouldBe("b95a4aa0-167a-4bc2-baf4-d43a776da1bd");
    }
    
    // TODO: Input diff based on stack name (will have to mock stack ref for all possible values)
    [Theory]
    [InlineData("dev")]
    [InlineData("prod")]
    public async Task Stack_ShouldDynamicallyApplyStackName(string stackName)
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(stackName));
        
        // TODO: assert on key vault name
    }
    
    [Fact]
    public async Task StackOutputs_ShouldOutputMockedValue()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
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
