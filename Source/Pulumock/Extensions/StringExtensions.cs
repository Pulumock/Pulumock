namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for working with strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts the first character of a string to lowercase, effectively transforming it to camelCase.
    /// NB! It will not work for more complex cases such as: "TRAnsformMe".
    /// </summary>
    /// <param name="name">The input string to convert.</param>
    /// <returns>
    /// The camelCase version of the input string. If the string is null, empty, whitespace, or already starts with a lowercase letter,
    /// the original string is returned unchanged.
    /// </returns>
    public static string ToCamelCase(this string name)
    {
        if (string.IsNullOrWhiteSpace(name) || char.IsLower(name[0]))
        {
            return name;
        }

        return char.ToLowerInvariant(name[0]) + name[1..];
    }
}
