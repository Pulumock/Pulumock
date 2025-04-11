using System.Reflection;
using Pulumi;
using Pulumock.Extensions;
using Shouldly;
using Xunit;

namespace Pulumock.Tests.Extensions;

public class MemberInfoExtensionsTests
{
    [Theory]
    [InlineData(typeof(TestCustomResource), "test:custom:TestCustomResource")]
    [InlineData(typeof(TestComponentResource), "test:custom:TestComponentResource")]
    public void MatchesPulumiTypeToken_ShouldReturnTrue_WhenTokenMatches(Type resourceType, string token)
    {
        bool result = resourceType.MatchesPulumiTypeToken(token);
        
        result.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData(typeof(TestCustomResource), "test:custom:TestComponentResource")]
    [InlineData(typeof(TestComponentResource), "test:custom:TestCustomResource")]
    [InlineData(typeof(TestCustomResource), "other:custom:resource")]
    [InlineData(typeof(TestComponentResource), "other:custom:resource")]
    public void MatchesPulumiTypeToken_ShouldReturnFalse_WhenTokenDoesNotMatch(Type resourceType, string token)
    {
        bool result = resourceType.MatchesPulumiTypeToken(token);
        
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MatchesPulumiTypeToken_ShouldReturnFalse_ForNullOrEmptyOrWhitespaceToken(string? token)
    {
        TypeInfo memberInfo = typeof(TestCustomResource).GetTypeInfo();

        bool result = memberInfo.MatchesPulumiTypeToken(token);

        result.ShouldBeFalse();
    }
    
    [ResourceType("test:custom:TestCustomResource", null)]
    private sealed class TestCustomResource(string name) : CustomResource("test:custom:TestCustomResource", name, null, null);
    
    [ResourceType("test:custom:TestComponentResource", null)]
    private sealed class TestComponentResource(string name) : ComponentResource("test:custom:TestComponentResource", name);
}
