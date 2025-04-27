using Example.ComponentResources;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.Resources;

namespace Example.Stacks;

internal static class CoreStack
{
    public static async Task<Dictionary<string, object?>> DefineResourcesAsync(string stackName)
    {
        var stackConfiguration = new StackConfiguration();

        var stackReference = new StackReference($"{stackConfiguration.StackReferenceOrgName}/{stackConfiguration.StackReferenceProjectName}/{stackName}");
        object? stackReferenceValue = await stackReference.GetValueAsync("microserviceManagedIdentityPrincipalId");
        if (stackReferenceValue is not string managedIdentity)
        {
            throw new InvalidCastException("Invalid stack ref: expected a string.");
        }

        var resourceGroup = new ResourceGroup("microservice-rg", new ResourceGroupArgs
        {
            ResourceGroupName = "microservice-rg",
            Location = stackConfiguration.Location
        });

        Vault keyVault = new KeyVaultWithSecretsComponentResource("microservice-kvws", new KeyVaultWithSecretsComponentResourceArgs
        {
            VaultName = $"microservice-kv-{stackName}",
            ResourceGroupName = resourceGroup.Name,
            TenantId = stackConfiguration.TenantId,
            Secrets = new InputMap<string>
            {
                {"Database--ConnectionString", stackConfiguration.DatabaseConnectionString}
            }
        }).KeyVault;
    
        GetRoleDefinitionResult roleDefinition = await GetRoleDefinition.InvokeAsync(new GetRoleDefinitionArgs
        {
            RoleDefinitionId = "b24988ac-6180-42a0-ab88-20f7382dd24c",
            Scope = $"/subscriptions/{stackConfiguration.SubscriptionId}"
        });
        
        // This exists to test multiple calls from the same provider function
        _ = await GetRoleDefinition.InvokeAsync(new GetRoleDefinitionArgs
        {
            RoleDefinitionId = "7f951dda-4ed3-4680-a7ca-43fe172d538d",
            Scope = $"/subscriptions/{stackConfiguration.SubscriptionId}"
        });
        
        _ = new RoleAssignment("microservice-ra-kvReader", new RoleAssignmentArgs
        {
            PrincipalId = managedIdentity,
            PrincipalType = PrincipalType.ServicePrincipal,
            RoleDefinitionId = roleDefinition.Id,
            Scope = keyVault.Id
        });
        
        return new Dictionary<string, object?>
        {
            {"keyVaultUri", keyVault.Properties.Apply(x => x.VaultUri) }
        };
    }
}
