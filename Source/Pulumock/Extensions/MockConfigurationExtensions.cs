using System.Collections.Immutable;
using Pulumock.Mocks.Enums;

namespace Pulumock.Extensions;

public static class MockConfigurationExtensions
{
    public static string Require(this ImmutableDictionary<string, object> mockConfigurations, PulumiConfigurationNamespace @namespace, string keyName)
    {
        string key = FormatKey(@namespace.Value, keyName);
        
        if(!mockConfigurations.TryGetValue(key, out object? value)
           || value is not string stringValue)
        {
            throw new KeyNotFoundException($"Configuration with key '{key}' was not found or is not of type string.");
        }
        
        return stringValue;
    }
    
    public static string? Get(this ImmutableDictionary<string, object> mockConfigurations, PulumiConfigurationNamespace @namespace, string keyName)
    {
        string key = FormatKey(@namespace.Value, keyName);
        
        if(!mockConfigurations.TryGetValue(key, out object? value)
           || value is not string stringValue)
        {
            return null;
        }
        
        return stringValue;
    }
    
    private static string FormatKey(string @namespace, string? keyName) =>
        string.IsNullOrWhiteSpace(keyName) ? @namespace : $"{@namespace}:{keyName}";
}
