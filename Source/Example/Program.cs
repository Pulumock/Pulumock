using Example.Stacks;
using Pulumi;

return await Deployment.RunAsync(async () => await CoreStack.DefineResourcesAsync());
