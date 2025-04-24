using System.Collections.Immutable;
using MainExample.Stacks;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.Resources;
using Pulumock.Extensions;
using Pulumock.Mocks.Builders;
using Pulumock.Mocks.Enums;
using Pulumock.TestFixtures;
using Shouldly;

namespace MainExample.Tests.WithPulumock;

public class ProgramTests
{
    private const string DevStackName = "dev";
    // Suggestion to test: root of stack (mock all component resources), extract logic to component resources (test in isolation)
    // TODO: full upsert, partial upsert, set by identifier or for entire type | (with/without methods for all) 
    // TODO: support both typed and non-typed builders and with() methods
    private readonly FixtureBuilder _fixtureBuilder = new FixtureBuilder()
        .WithMockConfiguration(new MockConfigurationBuilder()
            .WithConfiguration(PulumiConfigurationNamespace.AzureNative, "tenantId", "1f526cdb-1975-4248-ab0f-57813df294cb")
            .WithConfiguration(PulumiConfigurationNamespace.AzureNative, "subscriptionId", "f2f2c6e5-17c2-4dfa-913d-6509deb6becf")
            .WithConfiguration(PulumiConfigurationNamespace.AzureNative, "location", "swedencentral")
            .WithConfiguration(PulumiConfigurationNamespace.Default, "useKeyVaultWithSecretsComponentResource", "true")
            .WithSecretConfiguration(PulumiConfigurationNamespace.Default, "databaseConnectionString", "very-secret-value")
            .Build())
        .WithMockStackReference(new MockStackReferenceBuilder($"hoolit/Identity/{DevStackName}")
            .WithOutput("microserviceManagedIdentityPrincipalId", "b95a4aa0-167a-4bc2-baf4-d43a776da1bd")
            .Build())
        .WithMockResource(new MockResourceBuilder()
            .WithOutput<ResourceGroup>(x => x.AzureApiVersion, "2021-04-01")
            .Build<ResourceGroup>())
        .WithMockResource(new MockResourceBuilder()
            .WithOutput<GetVaultResult>(x => x.Properties.VaultUri, "https://mocked.vault.azure.net/")
            .Build<Vault>())
        .WithMockCall(new MockCallBuilder()
            .WithOutput<GetRoleDefinitionResult>(x => x.Id, "13a8e88e-f45f-432b-8b45-019997c19f27")
            .Build(typeof(GetRoleDefinition)));

    // TODO: test stack
    // - Config
    // - Secret
    // - Stack reference 
    //   - Resource dependency on stack
    // - Resource
    //   - Input only
    //   - Output only
    //   - Input & Output
    //   - Resource dependencies
    // - Creates Vault with/without component resource
    // - Input diff based on stack name
    // - Call
    //   - With/Without args
    //   - Resource dependencies on call
    // - Stack outputs
    
    // TODO: test component resource
    // - Required args
    // - Parent
    
    [Fact]
    public async Task DefineResourcesAsync_Config()
    {
        Fixture result = await _fixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(DevStackName));

        VaultPropertiesArgs keyVaultProperties = result.Inputs.RequireValue<VaultArgs, VaultPropertiesArgs>("microservice-kv-vault", x => x.Properties);
        
        keyVaultProperties.TenantId.ShouldBe("1f526cdb-1975-4248-ab0f-57813df294cb");
    }
    
    [Fact]
    public async Task DefineResourcesAsync_ConfigSecret()
    {
        Fixture result = await _fixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(DevStackName));
    }
    
    [Fact]
    public async Task DefineResourcesAsync_StackReference()
    {
        Fixture result = await _fixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(DevStackName));
    }
    
    [Fact]
    public async Task TestRun()
    {
        Fixture result = await _fixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(DevStackName));

        ResourceGroup resourceGroup = result.StackResources.GetResourceByLogicalName<ResourceGroup>("example-rg");
        result.StackOutputs.TryGetValue("exampleStackOutput", out object? exampleStackOutputValue);
        string input = result.Inputs.RequireValue<ResourceGroupArgs, string>("example-rg", x => x.ResourceGroupName);

        resourceGroup.ShouldNotBeNull();
        (await resourceGroup.Location.GetValueAsync()).ShouldBe("swedencentral");
        (await resourceGroup.AzureApiVersion.GetValueAsync()).ShouldBe("2021-04-01");
        (await resourceGroup.Tags.GetValueAsync())?.GetValueOrDefault("subscriptionId").ShouldBe("test-subscription-id");
        input.ShouldBe("test-rg-name");
        exampleStackOutputValue.ShouldBe("value");
    }
}
