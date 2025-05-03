namespace Pulumock.Utilities;

/// <summary>
/// Provides utilities for formatting Pulumi configuration keys.
/// </summary>
public static class ConfigurationKeyFormatter
{
    /// <summary>
    /// Formats a Pulumi configuration key using a namespace and optional key name.
    /// </summary>
    /// <param name="namespace">The configuration namespace (e.g., "azure-native" or "project").</param>
    /// <param name="keyName">The specific key within the namespace. If <c>null</c> or whitespace, only the namespace is returned.</param>
    /// <returns>The formatted configuration key string (e.g., "azure-native:tenantId").</returns>
    public static string Format(string @namespace, string? keyName) =>
        string.IsNullOrWhiteSpace(keyName) ? @namespace : $"{@namespace}:{keyName}";
}
