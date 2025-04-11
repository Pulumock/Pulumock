using Pulumi;
using Pulumock.Extensions;
using Shouldly;
using Xunit;

namespace Pulumock.Tests.Extensions;

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(TestCustomResource), "test:custom:TestCustomResource")]
    [InlineData(typeof(TestComponentResource), "test:custom:TestComponentResource")]
    public void MatchesResourceTypeToken_ShouldReturnTrue_WhenTokenMatches(Type resourceType, string token)
    {
        bool result = resourceType.MatchesResourceTypeToken(token);
        
        result.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData(typeof(TestCustomResource), "test:custom:TestComponentResource")]
    [InlineData(typeof(TestComponentResource), "test:custom:TestCustomResource")]
    [InlineData(typeof(TestCustomResource), "other:custom:resource")]
    [InlineData(typeof(TestComponentResource), "other:custom:resource")]
    public void MatchesResourceTypeToken_ShouldReturnFalse_WhenTokenDoesNotMatch(Type resourceType, string token)
    {
        bool result = resourceType.MatchesResourceTypeToken(token);
        
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(typeof(TestCustomResource), null)]
    [InlineData(typeof(TestCustomResource), "")]
    [InlineData(typeof(TestCustomResource), "    ")]
    [InlineData(typeof(TestComponentResource), null)]
    [InlineData(typeof(TestComponentResource), "")]
    [InlineData(typeof(TestComponentResource), "    ")]
    public void MatchesResourceTypeToken_ShouldReturnFalse_ForNullOrEmptyOrWhitespaceToken(Type resourceType, string? token)
    {
        bool result = resourceType.MatchesResourceTypeToken(token);

        result.ShouldBeFalse();
    }
    
    [ResourceType("test:custom:TestCustomResource", null)]
    private sealed class TestCustomResource(string name) : CustomResource("test:custom:TestCustomResource", name, null, null);
    
    [ResourceType("test:custom:TestComponentResource", null)]
    private sealed class TestComponentResource(string name) : ComponentResource("test:custom:TestComponentResource", name);
}
