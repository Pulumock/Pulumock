namespace Pulumock.Mocks.Enums;

/// <summary>
/// Represents configuration namespaces for Pulumi resource providers.
/// For additional namespaces, see the <see href="https://www.pulumi.com/registry/">Pulumi Registry</see>.
/// </summary>
public sealed class PulumiConfigurationNamespace
{
    /// <summary>
    /// Represents the default configuration namespace.
    /// In production, this is the name of the current Pulumi project; in tests, it defaults to "project".
    /// </summary>
    public static readonly PulumiConfigurationNamespace Default = new("project");
    public static readonly PulumiConfigurationNamespace Aws = new("aws");
    public static readonly PulumiConfigurationNamespace AzureClassic = new("azure");
    public static readonly PulumiConfigurationNamespace AzureNative = new("azure-native");
    public static readonly PulumiConfigurationNamespace GoogleCloudClassic = new("gcp");
    public static readonly PulumiConfigurationNamespace GoogleCloudNative = new("google-native");
    public static readonly PulumiConfigurationNamespace Kubernetes = new("kubernetes");
    
    /// <summary>
    /// The namespace value.
    /// </summary>
    public string Value { get; }
    
    private PulumiConfigurationNamespace(string value) 
        => Value = value;
}
