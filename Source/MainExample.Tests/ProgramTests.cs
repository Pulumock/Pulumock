using System.Collections.Immutable;
using System.Text.Json;
using MainExample.Stacks;
using Pulumi.AzureNative.Resources;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;
using Deployment = Pulumi.Deployment;
using Resource = Pulumi.Resource;

namespace MainExample.Tests;

public class ProgramTests
{
    public ProgramTests() => 
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "azure-native:location", "swedencentral" },
            { "project:exampleSecret", "verysecret" }
        }));
    
    [Fact]
    public async Task TestRun()
    {
        // TODO: should Outputs be named StackOutputs (?)
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> Outputs) result = await Deployment.TestAsync(new EmptyMocks(), new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(""));

        ResourceGroup resourceGroup = result.Resources
            .OfType<ResourceGroup>()
            .Single(x => x.GetResourceName().Equals("example-rg", StringComparison.Ordinal));

        string location = await OutputUtilities.GetValueAsync(resourceGroup.Location);
        
        resourceGroup.ShouldNotBeNull();
        location.ShouldBe("swedencentral");
    }
}
