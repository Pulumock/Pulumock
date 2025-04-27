using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.Authorization.Outputs;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.AzureNative.Resources;
using Pulumock.Mocks.Builders;
using Pulumock.Mocks.Enums;
using Pulumock.TestFixtures;

namespace Example.Tests.WithPulumock.Shared;


internal static class TestBase
{
    public const string DevStackName = "dev";
    
    public static FixtureBuilder GetBaseFixtureBuilder() =>
        new FixtureBuilder()
            .WithMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "tenantId", "1f526cdb-1975-4248-ab0f-57813df294cb")
            .WithMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "subscriptionId", "f2f2c6e5-17c2-4dfa-913d-6509deb6becf")
            .WithMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "location", "swedencentral")
            .WithMockStackConfiguration(PulumiConfigurationNamespace.Default, "stackReferenceOrgName", "hoolit")
            .WithMockStackConfiguration(PulumiConfigurationNamespace.Default, "stackReferenceProjectName", "StackReference")
            .WithMockStackConfiguration(PulumiConfigurationNamespace.Default, "databaseConnectionString", "very-secret-value")
            .WithMockStackReference(new MockStackReferenceBuilder($"hoolit/StackReference/{DevStackName}")
                .WithOutput("microserviceManagedIdentityPrincipalId", "b95a4aa0-167a-4bc2-baf4-d43a776da1bd")
                .Build())
            .WithMockResource(new MockResourceBuilder()
                .WithOutput<ResourceGroup>(x => x.AzureApiVersion, "2021-04-01")
                .WithOutput<ResourceGroup>(x => x.Location, "norway")
                .Build<ResourceGroup>())
            .WithMockResource(new MockResourceBuilder()
                .WithOutput<Vault, VaultPropertiesResponse>(x => x.Properties, p => p
                    .WithNestedOutput(x => x.VaultUri, "https://mocked.vault.azure.net/"))
                .Build<Vault>())
            .WithMockCall(new MockCallBuilder()
                .WithOutput<GetRoleDefinitionResult>(x => x.Id, "13a8e88e-f45f-432b-8b45-019997c19f27")
                .WithOutput<GetRoleDefinitionResult, PermissionResponse>(x => x.Permissions, p => 
                    p.WithNestedOutput(x => x.Condition, "condition"))
                .Build(typeof(GetRoleDefinition)));
}
