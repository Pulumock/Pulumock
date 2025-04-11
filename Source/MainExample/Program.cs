using MainExample;
using Pulumi.AzureNative.Resources;
using Deployment = Pulumi.Deployment;

return await Deployment.RunAsync(() =>
{
    var pulumiConfig = new PulumiConfig();
    
    _ = new ResourceGroup("resourceGroup", new()
    {
        Location = pulumiConfig.Location,
        ResourceGroupName = "exampleResourceGroup"
    });
});
