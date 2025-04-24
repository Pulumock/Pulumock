using System.Collections.Immutable;
using Pulumi.Testing;

namespace MainExample.Tests;

internal sealed class Mocks : IMocks
{
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        outputs.AddRange(args.Inputs);
        
        if (string.Equals(args.Type, "pulumi:pulumi:StackReference", StringComparison.Ordinal))
        {
            var mockOutputs = new Dictionary<string, object>();
            if (string.Equals("hoolit/Identity/dev", args.Name, StringComparison.Ordinal))
            {
                mockOutputs.Add("microserviceManagedIdentityPrincipalId", "b95a4aa0-167a-4bc2-baf4-d43a776da1bd");
            }

            outputs.Add("outputs", mockOutputs.ToImmutableDictionary());
            outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);
        }
        else
        {
            if (string.Equals("azure-native:resources:ResourceGroup", args.Type, StringComparison.Ordinal))
            {
                outputs.Add("location", "swedencentral");
                outputs.Add("azureApiVersion", "2021-04-01");
            }
            
            if (string.Equals("azure-native:keyvault:Vault", args.Type, StringComparison.Ordinal))
            {
                Dictionary<string, object> properties = outputs.TryGetValue("properties", out object? value) && value is Dictionary<string, object> existing
                    ? existing
                    : new Dictionary<string, object>();

                properties["vaultUri"] = "https://mocked.vault.azure.net/";
                outputs["properties"] = properties;
            }
                
            object physicalResourceName = outputs.GetValueOrDefault("name") ?? $"{GetLogicalResourceName(args.Name)}_physical";
            outputs.Add("name", physicalResourceName);
        }
        
        string resourceName = GetLogicalResourceName(args.Name);
        string resourceId = GetResourceId(args.Id, $"{resourceName}_id");
        
        return Task.FromResult<(string? id, object state)>((resourceId, outputs.ToImmutable()));
    }

    public Task<object> CallAsync(MockCallArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();

        if (string.Equals(GetCallToken(args.Token), "azure-native:authorization:getRoleDefinition", StringComparison.Ordinal))
        {
            outputs.Add("id", "13a8e88e-f45f-432b-8b45-019997c19f27");
        }
        
        return Task.FromResult<object>(outputs.ToImmutable());
    }
    
    private static string GetLogicalResourceName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
    
    private static string GetResourceId(string? id, string fallbackId) =>
        string.IsNullOrWhiteSpace(id) ? fallbackId : id;
    
    private static string GetCallToken(string? token) =>
        string.IsNullOrWhiteSpace(token) ? throw new ArgumentNullException(nameof(token)) : token;
}
