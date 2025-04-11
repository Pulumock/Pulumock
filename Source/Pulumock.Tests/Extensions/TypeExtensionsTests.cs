using Pulumi;
using Pulumock.Extensions;
using Shouldly;
using Xunit;

namespace Pulumock.Tests.Extensions;

public class TypeExtensionsTests
{
    private const string TestCustomResourceTypeToken = "test:custom:TestCustomResource";
    private const string TestComponentResourceTypeToken = "test:custom:TestComponentResource";
    
    [Theory]
    [InlineData(typeof(TestCustomResource), TestCustomResourceTypeToken)]
    [InlineData(typeof(TestComponentResource), TestComponentResourceTypeToken)]
    public void MatchesResourceTypeToken_ShouldReturnTrue_WhenTokenMatches(Type resourceType, string token)
    {
        bool result = resourceType.MatchesResourceTypeToken(token);
        
        result.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData(typeof(TestCustomResource), TestComponentResourceTypeToken)]
    [InlineData(typeof(TestComponentResource), TestCustomResourceTypeToken)]
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
    
    [ResourceType(TestCustomResourceTypeToken, null)]
    private sealed class TestCustomResource(string name) : CustomResource(TestCustomResourceTypeToken, name, null, null);
    
    [ResourceType(TestComponentResourceTypeToken, null)]
    private sealed class TestComponentResource(string name) : ComponentResource(TestComponentResourceTypeToken, name);
}
