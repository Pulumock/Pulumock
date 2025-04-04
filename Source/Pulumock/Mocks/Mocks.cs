using System.Collections.Immutable;
using Pulumi;
using Pulumi.Testing;
using Pulumock.Mocks.Constants;

namespace Pulumock.Mocks;

internal sealed class Mocks : IMocks
{
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        string resourceId = string.IsNullOrWhiteSpace(args.Id) ? $"{args.Name}_id" : args.Id;
        
        if (string.Equals(args.Type, ResourceTypeConstants.StackReference, StringComparison.Ordinal))
        {
            // TODO: implement
            var mocks = new Dictionary<string, object> { };
            
            outputs.Add("outputs", mocks);
            outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);
        }
        else
        {
            // TODO: implement
            outputs.Add("name", outputs.GetValueOrDefault("name") ?? $"{args.Name}_name");
        }
        
        outputs.AddRange(args.Inputs);
        
        return Task.FromResult<(string?, object)>((resourceId, outputs));
    }

    public Task<object> CallAsync(MockCallArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        return Task.FromResult<object>(outputs);
    }
}
