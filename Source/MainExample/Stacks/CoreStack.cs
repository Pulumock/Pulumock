using MainExample.ComponentResources;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.Resources;

namespace MainExample.Stacks;

internal static class CoreStack
{
    private const string OrgName = "hoolit";
    private const string IdentityProjectName = "Identity";

    public static async Task<Dictionary<string, object?>> DefineResourcesAsync(string stackName)
    {
        // Config
        var config = new PulumiConfig();

        // Stack ref
        var stackReference = new StackReference($"{OrgName}/{IdentityProjectName}/{stackName}");
        object? stackReferenceValue = await stackReference.GetValueAsync("microserviceManagedIdentityPrincipalId");
        if (stackReferenceValue is not string managedIdentity)
        {
            throw new InvalidCastException("Invalid stack ref: expected a string.");
        }

        // Resource
        var resourceGroup = new ResourceGroup("microservice-rg", new()
        {
            ResourceGroupName = "microservice-rg", // Not an output, so assert on input
            Location = config.Location // Input and Output
            // Outputs version which is not an input
        });

        Vault? keyVault;
        if (config.UseKeyVaultWithSecretsComponentResource)
        {
            keyVault = new KeyVaultWithSecretsComponentResource("microservice-kv", new()
            {
                VaultName = $"microservice-kv-{stackName}",
                ResourceGroupName = resourceGroup.Name,
                TenantId = config.TenantId,
                Secrets = new()
                {
                    {"Database--ConnectionString", config.DatabaseConnectionString}
                }
            }).KeyVault;
        }
        else
        {
            // Resource with dep
            keyVault = new Vault("microservice-kv-vault", new()
            {
                VaultName = $"microservice-kv-{stackName}", // Input with diff on stack name
                ResourceGroupName = resourceGroup.Name, // Dep on resource
                Properties = new VaultPropertiesArgs
                {
                    EnableRbacAuthorization = true,
                    Sku = new SkuArgs
                    {
                        Family = SkuFamily.A,
                        Name = SkuName.Standard
                    },
                    TenantId = config.TenantId
                }
            });
        
            _ = new Secret("microservice-kv-secret-Database--ConnectionString", new SecretArgs
            {
                SecretName = "Database--ConnectionString",
                Properties = new SecretPropertiesArgs
                {
                    Value = config.DatabaseConnectionString // Secret
                },
                ResourceGroupName = resourceGroup.Name, // Dep on resource
                VaultName = keyVault.Name // Dep on resource
            });   
        }
        
        ArgumentNullException.ThrowIfNull(keyVault);
        
        // Provider function (call)
        GetRoleDefinitionResult roleDefinition = await GetRoleDefinition.InvokeAsync(new GetRoleDefinitionArgs
        {
            RoleDefinitionId = "b24988ac-6180-42a0-ab88-20f7382dd24c",
            Scope = $"/subscriptions/{config.SubscriptionId}"
        });
        
        // For testing purposes only
        GetRoleDefinitionResult roleDefinitionSecondCall = await GetRoleDefinition.InvokeAsync(new GetRoleDefinitionArgs
        {
            RoleDefinitionId = "88fa32db-c830-43a9-88bc-fa482a8401e8",
            Scope = $"/subscriptions/{config.SubscriptionId}"
        });
        
        _ = new RoleAssignment("microservice-ra-kvReader", new RoleAssignmentArgs
        {
            PrincipalId = managedIdentity, // Dep on stack ref input
            PrincipalType = PrincipalType.ServicePrincipal,
            RoleDefinitionId = roleDefinition.Id, // Dep on call
            Scope = keyVault.Id // Dep on resource
        });

        // Stack outputs
        return new Dictionary<string, object?>
        {
            {"keyVaultUri", keyVault.Properties.Apply(x => x.VaultUri) }
        };
    }
}
