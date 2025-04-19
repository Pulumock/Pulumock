using Pulumi.AzureNative.Resources;
using Pulumock.Extensions;
using Pulumock.Mocks.Builders;
using Pulumock.Mocks.Enums;
using Pulumock.TestFixtures;
using Shouldly;

namespace MainExample.Tests.WithPulumock;

public class ProgramTests
{
    private readonly FixtureBuilder _fixtureBuilder = new FixtureBuilder()
        .WithMockConfiguration(new MockConfigurationBuilder()
            .WithConfiguration(PulumiConfigurationNamespace.AzureNative, "swedencentral", "location")
            .WithSecretConfiguration(PulumiConfigurationNamespace.Default, "very-secret-value", "exampleSecret")
            .Build())
        .WithMockStackReference(new MockStackReferenceBuilder("org/project/stack")
            .WithOutput("resourceGroupName", "test-rg-name")
            .Build())
        .WithMockResource(new MockResourceBuilder<ResourceGroup>()
            .WithOutput("azureApiVersion", "2021-04-01")
            .Build());

    [Fact]
    public async Task TestRun()
    {
        Fixture result = await _fixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());

        ResourceGroup resourceGroup = result.StackResources.GetResourceByLogicalName<ResourceGroup>("example-rg");
        result.StackOutputs.TryGetValue("exampleStackOutput", out object? exampleStackOutputValue);
        string input = result.Inputs.RequireValue<string>("example-rg", "resourceGroupName");
        
        resourceGroup.ShouldNotBeNull();
        (await resourceGroup.Location.GetValueAsync()).ShouldBe("swedencentral");
        (await resourceGroup.AzureApiVersion.GetValueAsync()).ShouldBe("2021-04-01");
        input.ShouldBe("test-rg-name");
        exampleStackOutputValue.ShouldBe("value");
    }
}
