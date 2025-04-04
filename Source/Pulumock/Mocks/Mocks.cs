using System.Collections.Immutable;
using Pulumi.Testing;
using Pulumock.Mocks.Constants;

namespace Pulumock.Mocks;

/// <summary>
/// Provides an implementation of <see cref="Pulumi.Testing.IMocks"/> for unit testing Pulumi stacks.
/// This class is responsible for mocking both resource creation and provider function (invoke) calls
/// so that Pulumi programs can be tested without deploying actual cloud infrastructure.
/// </summary>
internal sealed class Mocks : IMocks
{
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        string resourceId = string.IsNullOrWhiteSpace(args.Id) ? $"{args.Name}_id" : args.Id;
        
        outputs.AddRange(args.Inputs);
        
        if (string.Equals(args.Type, ResourceTypeTokenConstants.StackReference, StringComparison.Ordinal))
        {
            // TODO: implement ability to intercept specific tokens (args.Token)
            var mocks = new Dictionary<string, object> { };
            
            outputs.Add("outputs", mocks);
            outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);
        }
        else
        {
            // TODO: implement ability to intercept specific types (args.Type)
            object physicalResourceName = outputs.GetValueOrDefault("name") ?? $"{args.Name}_name";
            outputs.Add("name", physicalResourceName);
        }
        
        return Task.FromResult<(string?, object)>((resourceId, outputs));
    }

    public Task<object> CallAsync(MockCallArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        return Task.FromResult<object>(outputs);
    }
}
