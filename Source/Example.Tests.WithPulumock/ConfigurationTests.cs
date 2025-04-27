using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.Testing;
using Pulumock.Extensions;
using Pulumock.Mocks.Enums;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class ConfigurationTests : IConfigurationTests
{
    // Required -> With mocked config from base
    [Fact]
    public async Task Config_MockedConfigurationInResource()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(), 
                new TestOptions{ IsPreview = false, StackName = TestBase.StackName });
        
        ResourceSnapshot resourceSnapshot = fixture.ResourceSnapshots.Require("microservice-kvws-kv");
        string vaultTenantId = resourceSnapshot.RequireInputValue<VaultArgs, VaultPropertiesArgs, string>(
            x => x.Properties, 
            y => y.TenantId);

        string configTenantId = fixture.StackConfigurations.Require(PulumiConfigurationNamespace.AzureNative, "tenantId");
        
        vaultTenantId.ShouldBe(configTenantId);
    }
    
    // Required -> With modified config
    [Theory]
    [InlineData("75abe3bd-31dd-43be-bdfa-f4e937fac121")]
    [InlineData("e7c808e6-e111-4d82-b023-4075c2eee383")]
    [InlineData("4e5fdc58-8df1-43ab-b15f-ae7aeb7c45f7")]
    public async Task Config_MockedConfigurationInResource_Override(string tenantId)
    {
        const string configTenantIdKey = "tenantId";
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, configTenantIdKey, tenantId)
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(), 
                new TestOptions{ IsPreview = false, StackName = TestBase.StackName });
        
        ResourceSnapshot resourceSnapshot = fixture.ResourceSnapshots.Require("microservice-kvws-kv");
        
        string vaultTenantId = resourceSnapshot.RequireInputValue<VaultArgs, VaultPropertiesArgs, string>(
            x => x.Properties, 
            y => y.TenantId);

        string configTenantId = fixture.StackConfigurations.Require(PulumiConfigurationNamespace.AzureNative, configTenantIdKey);
        
        configTenantId.ShouldBe(tenantId);
        vaultTenantId.ShouldBe(tenantId);
        vaultTenantId.ShouldBe(configTenantId);
    }
    
    // Required -> Without mocked config - single
    [Fact]
    public async Task Config_MockedConfigurationInResource_OverrideRemoveSingle_ThrowsSinceRequired() =>
        await Should.ThrowAsync<RunException>(async () =>
        {
            _ = await TestBase.GetBaseFixtureBuilder()
                .WithoutMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "tenantId")
                .BuildAsync(async () => await CoreStack.DefineResourcesAsync(), 
                    new TestOptions{ IsPreview = false, StackName = TestBase.StackName });
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
                .BuildAsync(async () => await CoreStack.DefineResourcesAsync(), 
                    new TestOptions{ IsPreview = false, StackName = TestBase.StackName });
        });

    [Fact]
    public async Task Config_MockedSecretInResource()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(), 
                new TestOptions{ IsPreview = false, StackName = TestBase.StackName });
        
        Secret secret = fixture.StackResources.GetResourceByLogicalName<Secret>("microservice-kvws-secret-Database--ConnectionString");
        string configSecretValue = fixture.StackConfigurations.Require(PulumiConfigurationNamespace.Default, "databaseConnectionString");

        SecretPropertiesResponse secretProperties = await secret.Properties.GetValueAsync();
        secretProperties.Value.ShouldBe(configSecretValue);
    }
}
