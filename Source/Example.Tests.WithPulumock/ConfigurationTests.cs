using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumock.Extensions;
using Pulumock.Mocks.Enums;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class ConfigurationTests : IConfigurationTests
{
    private const string TenantId = "1f526cdb-1975-4248-ab0f-57813df294cb";
    
    // Required -> With mocked config from base
    [Fact]
    public async Task Config_MockedConfigurationInResource()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "tenantId", TenantId)
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        ResourceSnapshot resourceSnapshot = fixture.ResourceSnapshots.Require("microservice-kvws-kv");
        string vaultTenantId = resourceSnapshot.RequireInputValue<VaultArgs, VaultPropertiesArgs, string>(
            x => x.Properties, 
            y => y.TenantId);
        
        vaultTenantId.ShouldBe(TenantId);
    }
    
    // Required -> With modified config
    [Theory]
    [InlineData("75abe3bd-31dd-43be-bdfa-f4e937fac121")]
    [InlineData("e7c808e6-e111-4d82-b023-4075c2eee383")]
    [InlineData("4e5fdc58-8df1-43ab-b15f-ae7aeb7c45f7")]
    public async Task Config_MockedConfigurationInResource_Override(string tenantId)
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "tenantId", tenantId)
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        ResourceSnapshot resourceSnapshot = fixture.ResourceSnapshots.Require("microservice-kvws-kv");
        string vaultTenantId = resourceSnapshot.RequireInputValue<VaultArgs, VaultPropertiesArgs, string>(
            x => x.Properties, 
            y => y.TenantId);
        
        vaultTenantId.ShouldBe(tenantId);
    }
    
    // Required -> Without mocked config - single
    [Fact]
    public async Task Config_MockedConfigurationInResource_OverrideRemoveSingle_ThrowsSinceRequired() =>
        await Should.ThrowAsync<RunException>(async () =>
        {
            _ = await TestBase.GetBaseFixtureBuilder()
                .WithoutMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "tenantId")
                .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        });
    
    // Without mocked config -> all
    // TODO: This will fail when running parallel since using the same global ENV variable
    // - Maybe have to run this sequentially? 
    [Fact]
    public async Task Config_MockedConfigurationInResource_OverrideRemoveAll_ThrowsSinceRequired() =>
        await Should.ThrowAsync<RunException>(async () =>
        {
            _ = await TestBase.GetBaseFixtureBuilder()
                .ClearMockStackConfigurations()
                .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        });

    [Fact]
    public async Task Config_MockedSecretInResource()
    {
        const string databaseConnectionStringSecret = "very-secret-value";
        
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackConfiguration(PulumiConfigurationNamespace.Default, "databaseConnectionString", databaseConnectionStringSecret)
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        Secret secret = fixture.StackResources.Require<Secret>("microservice-kvws-secret-Database--ConnectionString");

        SecretPropertiesResponse secretProperties = await secret.Properties.GetValueAsync();
        secretProperties.Value.ShouldBe(databaseConnectionStringSecret);
    }
}
