using Pulumi;
using Pulumock.Extensions;
using Shouldly;
using Xunit;

namespace Pulumock.Tests.Extensions;

public class TypeExtensionsTests
{
    private const string TestCustomResourceTypeToken = "test:custom:TestCustomResource";
    private const string TestComponentResourceTypeToken = "test:custom:TestComponentResource";
    private const string TestProviderFunctionTypeToken = "test:custom/testProviderFunction:testProviderFunction";
    
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
    
    [Fact]
    public void MatchesCallTypeToken_ShouldReturnTrue_WhenTokenContainsFunctionTypeName()
    {
        Type functionType = typeof(TestProviderFunction);
        bool result = functionType.MatchesCallTypeToken(TestProviderFunctionTypeToken);

        result.ShouldBeTrue();
    }

    [Fact]
    public void MatchesCallTypeToken_ShouldReturnFalse_WhenTokenDoesNotContainFunctionTypeName()
    {
        Type functionType = typeof(TestProviderFunction);
        bool result = functionType.MatchesCallTypeToken("test:custom/testOther:testOther");

        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MatchesCallTypeToken_ShouldReturnFalse_WhenTokenIsNullOrWhitespace(string? token)
    {
        Type functionType = typeof(TestProviderFunction);
        bool result = functionType.MatchesCallTypeToken(token);

        result.ShouldBeFalse();
    }
    
    [ResourceType(TestCustomResourceTypeToken, null)]
    private sealed class TestCustomResource(string name) : CustomResource(TestCustomResourceTypeToken, name, null, null);
    
    [ResourceType(TestComponentResourceTypeToken, null)]
    private sealed class TestComponentResource(string name) : ComponentResource(TestComponentResourceTypeToken, name);

    private static class TestProviderFunction
    {
        public static Task<object> InvokeAsync()
            => Deployment.Instance.InvokeAsync<object>(TestProviderFunctionTypeToken, InvokeArgs.Empty);
    }
}
