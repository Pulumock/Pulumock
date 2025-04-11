using MainExample;

return await Pulumi.Deployment.RunAsync(async () => await CoreStack.DefineResourcesAsync());
