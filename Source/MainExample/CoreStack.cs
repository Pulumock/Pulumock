using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.Resources;
using Type = System.Type;

namespace MainExample;

internal static class CoreStack
{
    public static async Task<Dictionary<string, object?>> DefineResourcesAsync()
    {
        await Task.Delay(100);
        var config = new PulumiConfig();

        var stackReference = new StackReference("org/project/stack");
        object? stackReferenceValue = await stackReference.GetValueAsync("resourceGroupName");
        if (stackReferenceValue is not string resourceGroupName)
        {
            throw new InvalidCastException("Invalid stack ref: expected a string.");
        }
        
        GetClientConfigResult azureClientConfig = await GetClientConfig.InvokeAsync();

        Type ty = typeof(GetClientConfig);

        _ = new ResourceGroup("example-rg", new()
        {
            Location = config.Location,
            ResourceGroupName = resourceGroupName,
            Tags = new InputMap<string>()
            {
                {"subscriptionId", azureClientConfig.SubscriptionId}
            },
        });

        return new Dictionary<string, object?>
        {
            {"exampleStackOutput", "value" }
        };
    }
}
