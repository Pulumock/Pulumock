using Pulumi;

namespace Example;

internal sealed class StackConfiguration
{
    private readonly Config _defaultConfig = new();
    private readonly Config _azureNativeConfig = new("azure-native");
    
    public string TenantId => _azureNativeConfig.Require("tenantId");
    public string SubscriptionId => _azureNativeConfig.Require("subscriptionId");
    public string Location => _azureNativeConfig.Require("location");
    
    public string StackReferenceOrgName => _defaultConfig.Require("stackReferenceOrgName");
    public string StackReferenceProjectName => _defaultConfig.Require("stackReferenceProjectName");
    public Output<string> DatabaseConnectionString => _defaultConfig.RequireSecret("databaseConnectionString");
}
