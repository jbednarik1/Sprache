namespace Sprache
{
    /// <summary>
    ///     Represents a parser.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="input">The input to parse.</param>
    /// <returns>The result of the parser.</returns>
    public delegate IResult<T> Parser<out T>(IInput input);
}