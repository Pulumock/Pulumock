using MainExample.Stacks;
using Pulumi;

return await Deployment.RunAsync(async () => await CoreStack.DefineResourcesAsync(Deployment.Instance.StackName));
