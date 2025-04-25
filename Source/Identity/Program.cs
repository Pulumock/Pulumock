using Pulumi.AzureNative.ManagedIdentity;
using Pulumi.AzureNative.Resources;

return await Pulumi.Deployment.RunAsync(() =>
{
    var resourceGroup = new ResourceGroup("identity-rg", new()
    {
        ResourceGroupName = "identity-rg"
    });
    
    var managedIdentity = new UserAssignedIdentity("microservice-mi", new UserAssignedIdentityArgs
    {
        ResourceGroupName = resourceGroup.Name
    });
    
    return new Dictionary<string, object?>
    {
        {"microserviceManagedIdentityPrincipalId", managedIdentity.PrincipalId }
    };
});
