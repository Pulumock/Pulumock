using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.Resources;
using Deployment = Pulumi.Deployment;

namespace MainExample;

// TODO:
// - DependsOn
internal static class CoreStack
{
    private static readonly string Environment = Deployment.Instance.StackName;
    
    public static async Task<Dictionary<string, object?>> DefineResourcesAsync()
    {
        // Config
        var config = new PulumiConfig();

        // Stack ref
        var stackReference = new StackReference($"org/Identity/{Environment}");
        object? stackReferenceValue = await stackReference.GetValueAsync("managedIdentity");
        if (stackReferenceValue is not string managedIdentity)
        {
            throw new InvalidCastException("Invalid stack ref: expected a string.");
        }

        // Resource
        var resourceGroup = new ResourceGroup("microservice-rg", new()
        {
            ResourceGroupName = "microservice-rg" // Not an output, so assert on input
            // Outputs version which is not an input
        });

        Vault? keyVault;
        if (config.UseKeyVaultWithSecretsComponentResource)
        {
            keyVault = new KeyVaultWithSecretsComponentResource("microservice-kv", new()
            {
                VaultName = $"microservice-kv-{Environment}",
                ResourceGroupName = resourceGroup.Name,
                Secrets = new()
                {
                    {"Database--ConnectionString", config.DatabaseConnectionString}
                }
            }).KeyVault;
        }
        else
        {
            // Resource with dep
            keyVault = new Vault("microservice-kv", new()
            {
                VaultName = $"microservice-kv-{Environment}", // Input with diff on stack name
                ResourceGroupName = resourceGroup.Name, // Dep on resource
                Properties = new VaultPropertiesArgs
                {
                    EnableRbacAuthorization = true,
                    Sku = new SkuArgs
                    {
                        Family = SkuFamily.A,
                        Name = SkuName.Standard
                    }
                }
            });
        
            _ = new Secret("microservice-secret-databaseConnectionString", new SecretArgs
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
        
        GetRoleDefinitionResult roleDefinition = await GetRoleDefinition.InvokeAsync(new GetRoleDefinitionArgs
        {
            RoleDefinitionId = "b24988ac-6180-42a0-ab88-20f7382dd24c",
            Scope = $"/subscriptions/{config.SubscriptionId}"
        });
        
        _ = new RoleAssignment("kv-reader-access", new RoleAssignmentArgs
        {
            PrincipalId = managedIdentity, // Dep on stack ref input
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
