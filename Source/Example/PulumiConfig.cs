using Pulumi;

namespace Example;

internal sealed class PulumiConfig
{
    private readonly Config _defaultConfig = new();
    private readonly Config _azureNativeConfig = new("azure-native");
    
    public string TenantId => _azureNativeConfig.Require("tenantId");
    public string SubscriptionId => _azureNativeConfig.Require("subscriptionId");
    public string Location => _azureNativeConfig.Require("location");
    
    public bool UseKeyVaultWithSecretsComponentResource => _defaultConfig.RequireBoolean("useKeyVaultWithSecretsComponentResource");
    public Output<string> DatabaseConnectionString => _defaultConfig.RequireSecret("databaseConnectionString");
}
