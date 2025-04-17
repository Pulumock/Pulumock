using Pulumi.AzureNative.Resources;

namespace MainExample;

internal static class CoreStack
{
    public static async Task<Dictionary<string, object?>> DefineResourcesAsync()
    {
        await Task.Delay(100);
        var config = new PulumiConfig();

        _ = new ResourceGroup("example-rg", new()
        {
            Location = config.Location,
            ResourceGroupName = "exampleResourceGroup"
        });

        return new Dictionary<string, object?>();
    }
}
