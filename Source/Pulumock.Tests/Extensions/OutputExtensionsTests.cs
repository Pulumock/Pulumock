using Pulumi;
using Shouldly;
using Xunit;
using Pulumock.Extensions;

namespace Pulumock.Tests.Extensions;

public class OutputExtensionsTests
{
    [Fact]
    public async Task GetValueAsync_ShouldReturnString()
    {
        const string value = "value";
        var output = Output.Create(value);

        string result = await output.GetValueAsync();

        result.ShouldBe(value);
    }

    [Fact]
    public async Task GetValueAsync_ShouldReturnInt()
    {
        const int value = 42;
        var output = Output.Create(value);
        
        int result = await output.GetValueAsync();

        result.ShouldBe(value);
    }

    [Fact]
    public async Task GetValueAsync_ShouldReturnComplexObject()
    {
        var value = new Person("Piralbin", 20);
        var output = Output.Create(value);

        Person result = await output.GetValueAsync();

        result.ShouldBeEquivalentTo(new Person("Piralbin", 20));
    }

    private sealed record Person(string Name, int Age);
}
