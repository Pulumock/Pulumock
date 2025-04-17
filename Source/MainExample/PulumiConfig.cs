using Pulumi;

namespace MainExample;

internal sealed class PulumiConfig
{
    private readonly Config _defaultConfig = new();
    private readonly Config _azureNativeConfig = new("azure-native");
    
    public Output<string> ExampleSecret => _defaultConfig.RequireSecret("exampleSecret");
    
    public string Location => _azureNativeConfig.Require("location");
}
