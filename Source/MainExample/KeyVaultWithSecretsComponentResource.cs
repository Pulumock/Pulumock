using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;

namespace MainExample;

internal sealed class KeyVaultWithSecretsComponentResource : ComponentResource
{
    public KeyVaultWithSecretsComponentResource(string name, KeyVaultWithSecretsComponentResourceArgs args, ComponentResourceOptions? options = null)
        : base("org:components:KeyVaultWithSecretsComponentResource", name, options)
    {
        var keyVault = new Vault($"{name}-vault", new()
        {
            VaultName = args.VaultName,
            ResourceGroupName = args.ResourceGroupName,
            Properties = new VaultPropertiesArgs
            {
                EnableRbacAuthorization = true,
                Sku = new SkuArgs
                {
                    Family = SkuFamily.A,
                    Name = SkuName.Standard
                },
                TenantId = args.TenantId
            }
        }, new() { Parent = this });
        
        args.Secrets.Apply(secrets =>
        {
            foreach (KeyValuePair<string, string> kv in secrets)
            {
                _ = new Secret($"{name}-secret-{kv.Key}", new SecretArgs
                {
                    SecretName = kv.Key,
                    Properties = new SecretPropertiesArgs
                    {
                        Value = kv.Value
                    },
                    ResourceGroupName = args.ResourceGroupName,
                    VaultName = keyVault.Name
                }, new CustomResourceOptions { Parent = this });
            }

            return secrets;
        });

        KeyVault = keyVault;
        RegisterOutputs(new Dictionary<string, object?>
        {
            { "keyVault", keyVault }
        });
    }
    
    public Vault KeyVault { get; }
}

internal sealed class KeyVaultWithSecretsComponentResourceArgs : ResourceArgs
{
    [Input("vaultName", required: true)]
    public required Input<string> VaultName { get; init; }
    
    [Input("resourceGroupName", required: true)]
    public required Input<string> ResourceGroupName { get; init; }
    
    [Input("tenantId", required: true)]
    public required Input<string> TenantId { get; init; }
    
    [Input("secrets")]
    public required InputMap<string> Secrets { get; init; }
}
