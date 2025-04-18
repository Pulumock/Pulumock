using System.Collections.Immutable;
using Pulumi.AzureNative.Authorization;
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
            .WithConfiguration(PulumiConfigurationNamespace.AzureNative, "location", "swedencentral")
            .WithSecretConfiguration(PulumiConfigurationNamespace.Default, "exampleSecret", "very-secret-value")
            .Build())
        .WithMockStackReference(new MockStackReferenceBuilder("org/project/stack")
            .WithOutput("resourceGroupName", "test-rg-name")
            .Build())
        .WithMockResource(new MockResourceBuilder<ResourceGroup>()
            .WithOutput(x => x.AzureApiVersion, "2021-04-01")
            .Build())
        .WithMockCall(new MockCallBuilder()
            .WithOutput<GetClientConfigResult>(x => x.SubscriptionId, "test-subscription-id")
            .Build(typeof(GetClientConfig)));

    [Fact]
    public async Task TestRun()
    {
        Fixture result = await _fixtureBuilder
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());

        ResourceGroup resourceGroup = result.StackResources.GetResourceByLogicalName<ResourceGroup>("example-rg");
        result.StackOutputs.TryGetValue("exampleStackOutput", out object? exampleStackOutputValue);
        string input = result.Inputs.RequireValue<ResourceGroupArgs, string>("example-rg", x => x.ResourceGroupName);

        resourceGroup.ShouldNotBeNull();
        (await resourceGroup.Location.GetValueAsync()).ShouldBe("swedencentral");
        (await resourceGroup.AzureApiVersion.GetValueAsync()).ShouldBe("2021-04-01");
        (await resourceGroup.Tags.GetValueAsync())?.GetValueOrDefault("subscriptionId").ShouldBe("test-subscription-id");
        input.ShouldBe("test-rg-name");
        exampleStackOutputValue.ShouldBe("value");
    }
}
