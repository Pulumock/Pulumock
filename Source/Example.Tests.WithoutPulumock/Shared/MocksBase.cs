using System.Collections.Immutable;
using Pulumi.Testing;

namespace Example.Tests.WithoutPulumock.Shared;

internal class MocksBase : IMocks
{
    public ImmutableList<EnrichedResource> EnrichedResources => ProtectedEnrichedResources.ToImmutableList();
    public ImmutableList<EnrichedCall> EnrichedCalls => ProtectedEnrichedCalls.ToImmutableList();
    
    public virtual Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        outputs.AddRange(args.Inputs);
        
        if (string.Equals(args.Type, "pulumi:pulumi:StackReference", StringComparison.Ordinal))
        {
            var mockOutputs = new Dictionary<string, object>();
            if (string.Equals($"hoolit/StackReference/{DevStackName}", args.Name, StringComparison.Ordinal) 
                || string.Equals($"hoolit/StackReference/{ProdStackName}", args.Name, StringComparison.Ordinal))
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

        ProtectedEnrichedResources.Add(new EnrichedResource(resourceName, args.Inputs));
        
        return Task.FromResult<(string? id, object state)>((resourceId, outputs.ToImmutable()));
    }

    public virtual Task<object> CallAsync(MockCallArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        outputs.AddRange(args.Args);
        
        if (string.Equals(GetCallToken(args.Token), "azure-native:authorization:getRoleDefinition", StringComparison.Ordinal))
        {
            if (args.Args.TryGetValue("roleDefinitionId", out object? value) 
                && value is string existing 
                && string.Equals(existing, "b24988ac-6180-42a0-ab88-20f7382dd24c", StringComparison.Ordinal))
            {
                outputs.Add("id", "b24988ac-6180-42a0-ab88-20f7382dd24c");
            }
            else
            {
                outputs.Add("id", "dceab7c7-8a53-4a0e-b7f4-3416fac8ea4f");
            }
        }
        
        ImmutableDictionary<string, object> finalOutputs = outputs.ToImmutable();
        
        ProtectedEnrichedCalls.Add(new EnrichedCall(GetCallToken(args.Token), args.Args, finalOutputs));
        
        return Task.FromResult<object>(finalOutputs);
    }
    
    public const string DevStackName = "dev";
    public const string ProdStackName = "prod";
    
    protected readonly List<EnrichedResource> ProtectedEnrichedResources = [];
    protected readonly List<EnrichedCall> ProtectedEnrichedCalls = [];
    
    protected static string GetLogicalResourceName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
    
    protected static string GetResourceId(string? id, string fallbackId) =>
        string.IsNullOrWhiteSpace(id) ? fallbackId : id;
    
    protected static string GetCallToken(string? token) =>
        string.IsNullOrWhiteSpace(token) ? throw new ArgumentNullException(nameof(token)) : token;
}

internal sealed record EnrichedResource(string LogicalName, ImmutableDictionary<string, object> Inputs);

internal sealed record EnrichedCall(string Token, ImmutableDictionary<string, object> Inputs, ImmutableDictionary<string, object> Outputs);
