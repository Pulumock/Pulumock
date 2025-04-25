using System.Collections.Immutable;
using Pulumock.Mocks.Enums;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A fluent builder for creating a <see cref="MockConfiguration"/>.
/// </summary>
public class MockConfigurationBuilder
{
    private readonly Dictionary<string, object> _configurations = new();
    
    /// <summary>
    /// Adds a plain configuration key and value.
    /// Also supports <see href="https://www.pulumi.com/docs/iac/concepts/config/#structured-configuration">structured configuration</see>.
    /// </summary>
    /// <param name="key">The full configuration key, including namespace if needed.</param>
    /// <param name="value">The value to assign to the key.</param>
    public MockConfigurationBuilder WithConfiguration(string key, object value)
    {
        _configurations.Add(key, value);
        return this;
    }
    
    /// <summary>
    /// Adds a plain configuration secret key and value.
    /// </summary>
    /// <param name="key">The full configuration key, including namespace if needed.</param>
    /// <param name="secret">The secret value to assign to the key.</param>
    public MockConfigurationBuilder WithSecretConfiguration(string key, string secret)
    {
        WithConfiguration(key, secret);

        return this;
    }

    /// <summary>
    /// Adds a configuration key and value under a specific namespace.
    /// Also supports <see href="https://www.pulumi.com/docs/iac/concepts/config/#structured-configuration">structured configuration</see>.
    /// </summary>
    /// <param name="namespace">The configuration namespace (e.g., "project", "azure-native").</param>
    /// <param name="value">The configuration value.</param>
    /// <param name="keyName">The optional key name within the namespace.</param>
    public MockConfigurationBuilder WithConfiguration(PulumiConfigurationNamespace @namespace, string keyName, object value)
    {
        string key = FormatKey(@namespace.Value, keyName);
        
        WithConfiguration(key, value);
        
        return this;
    }
    
    /// <summary>
    /// Adds a configuration key and secret value under a specific namespace.
    /// The value is wrapped in a structure recognized by Pulumi as a secret.
    /// </summary>
    /// <param name="namespace">The configuration namespace (e.g., "project", "azure-native").</param>
    /// <param name="value">The secret value.</param>
    /// <param name="keyName">The optional key name within the namespace.</param>
    public MockConfigurationBuilder WithSecretConfiguration(PulumiConfigurationNamespace @namespace, string keyName, string value)
    {
        string key = FormatKey(@namespace.Value, keyName);
        
        WithSecretConfiguration(key, value);
        
        return this;
    }
    
    /// <summary>
    /// Builds the <see cref="MockConfiguration"/> mock.
    /// </summary>
    public MockConfiguration Build() =>
        new(_configurations.ToImmutableDictionary());
    
    private static string FormatKey(string @namespace, string? keyName) =>
        string.IsNullOrWhiteSpace(keyName) ? @namespace : $"{@namespace}:{keyName}";
}
