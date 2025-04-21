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
                }
            }
        }, new() { Parent = this });

        foreach (KeyValuePair<string, Input<string>> kv in args.Secrets)
        {
            _ = new Secret("microservice-example-secret", new SecretArgs
            {
                SecretName = kv.Key,
                Properties = new SecretPropertiesArgs
                {
                    Value = kv.Value
                },
                ResourceGroupName = args.ResourceGroupName,
                VaultName = keyVault.Name
            }, new() { Parent = this });
        }

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
    
    [Input("secrets")]
    public required InputMap<string> Secrets { get; init; }
}
