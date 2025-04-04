using Pulumi;

namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for working with Pulumi <see cref="Output{T}"/> values in testing contexts.
/// </summary>
public static class OutputExtensions
{
    /// <summary>
    /// Asynchronously retrieves the resolved value from a Pulumi <see cref="Output{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the Output.</typeparam>
    /// <param name="output">The Pulumi Output to unwrap.</param>
    /// <returns>A <see cref="Task{T}"/> that completes with the resolved value of the Output.</returns>
    public static Task<T> GetValueAsync<T>(this Output<T> output)
    {
        var tcs = new TaskCompletionSource<T>();
        output.Apply(value =>
        {
            tcs.SetResult(value);
            return value;
        });
        
        return tcs.Task;
    }
}
